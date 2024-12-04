import os
from datetime import datetime
from slack_sdk import WebClient
from slack_sdk.errors import SlackApiError
from dotenv import load_dotenv

load_dotenv(override=True)

SLACK_BOT_TOKEN = os.getenv("SLACK_BOT_TOKEN")
client = WebClient(token=SLACK_BOT_TOKEN)

def auth_test():
    try:
        response = client.auth_test()
        return response
    except SlackApiError as e:
        print(e)
        print(f"Error: {e.response['error']}")

def get_thread_comments(channel_id, thread_ts):
    try:
        response = client.conversations_replies(
            channel=channel_id,
            ts=thread_ts,
            limit=1000
        )
        messages = response['messages']
        return messages
    except SlackApiError as e:
        print(e)
        print(f"Error fetching thread: {e.response['error']}")
        return []

def parse_comments(comments):
    result = []
    for comment in comments:
        parsed_comment = {
            "id": comment['client_msg_id'],
            "content": comment['text'],
            "user_id": comment['user'],
            "ts": comment['ts'],
            "comment_datetime": datetime.fromtimestamp(float(comment['ts'])),
        }
        image_urls = []
        video_urls = []
        
        # attachments
        if attachments := comment.get('attachments'):
            for attachment in attachments:
                # image_url
                if 'image_url' in attachment:
                    image_url = attachment['image_url']
                    image_urls.append(image_url)
        
        # files
        if files := comment.get('files'):
            for file in files:
                if mimetype := file.get('mimetype'):
                    # image url
                    if mimetype.startswith('image'):
                        image_url = file['url_private']
                        image_urls.append(image_url)
                    
                    # video url
                    elif mimetype.startswith('video'):
                        video_url = file['url_private']
                        video_urls.append(video_url)
        
        parsed_comment['image_urls'] = image_urls
        parsed_comment['video_urls'] = video_urls
        result.append(parsed_comment)
    
    return result
        

        

# def check_images(comments):
    