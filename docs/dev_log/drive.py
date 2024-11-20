import os, json, uuid
import requests
import mimetypes
from googleapiclient.discovery import build
from googleapiclient.http import MediaFileUpload
from google.oauth2 import service_account
from wasabi import msg

from dotenv import load_dotenv
from utils import *

load_dotenv(override=True)
sprint_no = os.getenv("TEAM_SPRINT_NO")

SCOPES = [os.getenv("GOOGLE_DRIVE_SCOPE")]
SERVICE_ACCOUNT_FILE = os.getenv("GOOGLE_SERVICE_ACCOUNT_FILE")
GOOGLE_DRIVE_FOLDER_ID = os.getenv("GOOGLE_DRIVE_FOLDER_ID")

def build_drive_service():
    creds = None
    
    creds = service_account.Credentials.from_service_account_file(
        SERVICE_ACCOUNT_FILE, scopes=SCOPES
    )
    return build("drive", "v3", credentials=creds)

def get_or_create_subfolder(drive_service, parent_id, folder_name):
    # get or create the subfolder
    response = drive_service.files().list(
        q=f"name='{folder_name}' and trashed=false and '{parent_id}' in parents",
        fields="files(id, name)"
    ).execute()
    
    folder_id = None
    if not response["files"]:
        # create the folder
        file_metadata = {
            "name": folder_name,
            "mimeType": "application/vnd.google-apps.folder",
            "parents": [parent_id],
        }
        folder = drive_service.files().create(
            body=file_metadata,
            fields="id",
            supportsAllDrives=True
        ).execute()
        folder_id = folder.get("id")
    else:
        folder_id = response["files"][0].get("id")
    return folder_id

def get_nested_folder_id(drive_service, team_drive_id, folder_name):
    folder_names = str(folder_name).replace("\\" , "/").split("/")
    parent_id = team_drive_id
    for folder_name in folder_names:
        parent_id = get_or_create_subfolder(drive_service, parent_id, folder_name)
    return parent_id

def download_image(image_url):
    response = requests.get(
        image_url,
        headers={"Authorization": f"Bearer {os.getenv('SLACK_BOT_TOKEN')}"} # slack bot token
    )
    if response.status_code == 200:
        return response.content
    else:
        msg.fail(f"[drive.py] Failed to download image: {image_url}")
        return None

def donwload_image_to_local(image_url, image_path, tmp_dir="tmp"):
    response = requests.get(
        image_url,
        headers={"Authorization": f"Bearer {os.getenv('SLACK_BOT_TOKEN')}"} # slack bot token
    )
    if response.status_code == 200:
        if not os.path.exists(tmp_dir):
            os.makedirs(tmp_dir)
        
        with open(os.path.join(tmp_dir, image_path), "wb") as f:
            f.write(response.content)
        return os.path.join(tmp_dir, image_path)
    else:
        msg.fail(f"[drive.py] Failed to download image: {image_url}")
        return None

def download_video(video_url):
    response = requests.get(
        video_url,
        headers={"Authorization": f"Bearer {os.getenv('SLACK_BOT_TOKEN')}"},
        stream=True
    )
    if response.status_code == 200:
        return response.content
    else:
        msg.fail(f"[drive.py] Failed to download video: {video_url}, Status Code: {response.status_code}")
        return None

def download_video_to_local(video_url, video_path, tmp_dir="tmp"):
    response = requests.get(
        video_url,
        headers={"Authorization": f"Bearer {os.getenv('SLACK_BOT_TOKEN')}"},
        stream=True
    )
    if response.status_code == 200:
        if not os.path.exists(tmp_dir):
            os.makedirs(tmp_dir)
        
        with open(os.path.join(tmp_dir, video_path), "wb") as f:
            for chunk in response.iter_content(chunk_size=8192):
                f.write(chunk)
        return os.path.join(tmp_dir, video_path)
    else:
        msg.fail(f"[drive.py] Failed to download video: {video_url}, Status Code: {response.status_code}")
        return None

def upload_file(drive_service, file_path, folder_id):
    if not drive_service:
        msg.fail("[drive.py] Failed to build drive service")
        return
    
    file_metadata = {
        "name": os.path.basename(file_path),
        "parents": [folder_id]
    }
    media = MediaFileUpload(file_path, resumable=True)
    
    file = drive_service.files().create(
        body=file_metadata,
        media_body=media,
        fields="id",
        supportsAllDrives=True
    ).execute()
    
    return file.get("id")

def create_shareable_link(drive_service, file_id):
    drive_service.permissions().create(
        fileId=file_id,
        body={"role": "reader", "type": "anyone"},
        fields="id",
    ).execute()
    
    file = drive_service.files().get(
        fileId=file_id,
        fields="webViewLink"
    ).execute()
    return file.get("webViewLink")

def generate_filename_from_url(url):
    filename = os.path.basename(url)
    filename_without_ext, ext = os.path.splitext(filename)
    return f"{filename_without_ext}_{uuid.uuid4().hex[:6]}{ext}" # add random string to avoid duplication

def upload_from_comment(comment, tmp_dir="tmp"):
    drive_service = build_drive_service()
    if not drive_service:
        msg.fail("[drive.py] Failed to build drive service")
        return
    tmp_files = []
    
    date_str = get_date_from_comment(comment)
    initials = get_team_initials(comment['user_id'])
    
    full_path = os.path.join(
        f"Sprint{sprint_no}", "DevLogs", f"{date_str}_{initials}"
    )
    
    folder_id = get_nested_folder_id(drive_service, GOOGLE_DRIVE_FOLDER_ID, str(full_path).replace("\\" , "/"))
    
    # upload json
    if json_data := comment.get("json_data"):
        json_file_name = f"{date_str}_{initials}_data.json"
        tmp_json_path = os.path.join(tmp_dir, json_file_name)
        tmp_files.append(tmp_json_path)
        
        # temporary save the json data
        with open(tmp_json_path, "w", encoding="utf-8") as f:
            json.dump(json_data, f, indent=4, ensure_ascii=False)
            
        # upload the json file
        file_id = upload_file(drive_service, tmp_json_path, folder_id)
        
        msg.good(f"[drive.py] Uploaded json {json_file_name} to {full_path}. File ID: {file_id}")
    
    
    # upload image
    image_urls = comment.get('image_urls', [])
    for image_url in image_urls:
        # download the image
        image_filename = generate_filename_from_url(image_url)
        
        tmp_image_path = donwload_image_to_local(image_url, image_filename, tmp_dir)
        if not tmp_image_path:
            continue
        tmp_files.append(tmp_image_path)
        
        file_id = upload_file(drive_service, tmp_image_path, folder_id)
        
        msg.good(f"[drive.py] Uploaded image {image_filename} to {full_path}. File ID: {file_id}")
    
    # upload video
    video_urls = comment.get('video_urls', [])
    for video_url in video_urls:
        # download the video
        video_filename = generate_filename_from_url(video_url)
        
        tmp_video_path = download_video_to_local(video_url, video_filename, tmp_dir)
        if not tmp_video_path:
            continue
        tmp_files.append(tmp_video_path)
        
        file_id = upload_file(drive_service, tmp_video_path, folder_id)
        
        msg.good(f"[drive.py] Uploaded video {video_filename} to {full_path}. File ID: {file_id}")
        
    # share the folder
    shareable_link = create_shareable_link(drive_service, folder_id)
    comment["share_link"] = shareable_link
