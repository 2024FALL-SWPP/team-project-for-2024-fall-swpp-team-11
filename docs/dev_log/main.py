import os, json, random
from pprint import pprint
from dotenv import load_dotenv
from slack import get_thread_comments, auth_test, parse_comments
from comment import parse_texts, parse_text
from drive import upload_from_comment
from wiki import *
from wasabi import msg

from utils import *

load_dotenv(override=True)

PARSE_TEXT = True
UPLOAD_DRIVE = True
PUSH_REPO = True

if __name__ == '__main__':
    channel_id = os.getenv("SLACK_CHANNEL_ID")
    thread_ts = os.getenv("SLACK_THREAD_TS")
    tmp_dir = os.path.join(os.getcwd(), "tmp", "data")
    backup_dir = os.path.join(os.getcwd(), "tmp", "backup")
    os.makedirs(tmp_dir, exist_ok=True)
    os.makedirs(backup_dir, exist_ok=True)
    

    local_repo_path = os.path.join(os.getcwd(), "tmp", "repo.wiki")
    repo = os.getenv("GITHUB_WIKI_REPO_URL")
    md_file = os.getenv("GITHUB_WIKI_PAGE_NAME")
    
    # fetch comments
    comments = get_thread_comments(channel_id, thread_ts)
    parsed_comments = parse_comments(comments)
    parsed_comments.sort(key=lambda x: x['comment_datetime'])
    parsed_comments = parsed_comments[1:] # remove the first comment, which is the body of the thread
    msg.info(f"Fetched all the comments of the thread({thread_ts}) in channel({channel_id}).\n Total comments: {len(parsed_comments)}")
    
    # parse comment contents
    # Avoid bottleneck by parsing the texts in parallels
    texts = [comment['content'] for comment in parsed_comments]
    for i, (parsed_comment, text) in enumerate(zip(parsed_comments, texts)):
        msg.info(f"({i+1} / {len(parsed_comments)}) Start processing comment ({parsed_comment['id']})")
            
        if PARSE_TEXT:
            try:
                parsed_text = parse_text(text)
                parsed_comment["json_data"] = parsed_text
                msg.good(f"text parsed to json")
            except Exception as e:
                parsed_comment["json_data"] = {}
                msg.fail(f"Failed to parse text to json: {e}")
                
            # TODO better way
            if not parsed_comment['json_data'].get("workers"):
                parsed_comment['json_data']['workers'] = [get_team_name(parsed_comment['user_id'])]
        
        if UPLOAD_DRIVE:
            try:
                upload_from_comment(parsed_comment, tmp_dir)
                msg.good(f"image uploaded to drive")
            except Exception as e:
                msg.fail(f"Failed to upload image to drive: {e}")
    
    # persist parsed comments
    json_name = f"parsed_comments_{thread_ts}.json"
    while os.path.exists(os.path.join(backup_dir, json_name)):
        json_name = f"parsed_comments_{thread_ts}_{random.randint(0, 1000)}.json"
    
    try:
        msg.info(f"Saving parsed comments to {json_name}")
        serialized_comments = [serialize_comment(comment) for comment in parsed_comments]
        with open(os.path.join(backup_dir, json_name), "w", encoding="utf-8") as f:
            json.dump(serialized_comments, f, indent=4, ensure_ascii=False, sort_keys=True)
        msg.good(f"parsed comments saved to {json_name}")
    except Exception as e:
        msg.fail(f"Failed to save parsed comments: {e}")
        msg.info(f"Instead save as text file")
        try:
            txt_file_name_without_ext = os.path.splitext(json_name)[0]
            txt_file_name = f"{txt_file_name_without_ext}.txt"
            with open(os.path.join(backup_dir, txt_file_name), "w", encoding="utf-8") as f:
                f.write("\n".join([str(comment) for comment in parsed_comments]))
            msg.good(f"parsed comments saved to {txt_file_name}")
        except Exception as e:
            msg.fail(f"Failed to save parsed comments as text file: {e}")

    # # remove tmp directory
    # os.rmdir(tmp_dir)
    
    # upload to wiki
    if PUSH_REPO:
        msg.info("Start updating wiki")
        try:
            pull_repo(repo, local_repo_path)
            for i, comment in enumerate(parsed_comments):
                update_markdown(local_repo_path, md_file, comment, overwrite=(i==0), idx=i)
            push_repo(local_repo_path, "Update devlogs")
            msg.good("Wiki updated")
        except Exception as e:
            msg.fail(f"Failed to update wiki: {e}")