import os, json, time
from dotenv import load_dotenv
from openai import OpenAI

load_dotenv(override=True)

INSTRUCTION_PROMPT = lambda text: {
    "role": "system",
    "content": f"""
Extract the following fields from the text and return ad JSON.
Do not include any additional information. Just return the fields as JSON.
You should return the JSON just as it is in the text.
Your response will be used as JSON data.
- date (string, format: "YY/MM/DD")
- task_id (list of strings)
- workers (list of strings)
- status (string)
- working_time (string)
- location (list of strings)
- roles (list of strings)
- work_summary (string)
- notes (string)
- reference_links (list of strings. You should parse correct URLs from the text)

Text:
{text}
"""}

def get_openai_response(prompt):
    client = OpenAI(
        api_key=os.getenv("OPENAI_API_KEY")
    )
    try:
        response = client.chat.completions.create(
            messages=[prompt],
            model="gpt-4o-mini",
            temperature=0,
        )
        return response.choices[0].message.content
    except Exception as e:
        print(e)
        return None

def _parse_text_with_retry(prompt, max_retries=3, delay=1):
    for attempt in range(max_retries):
        try:
            response = get_openai_response(prompt) 
            parsed_data = json.loads(response)
            return parsed_data
        except (json.JSONDecodeError, Exception) as e:
            print(f"Attempt {attempt + 1} failed for prompt '{prompt}': {e}")
            if attempt < max_retries - 1:
                time.sleep(delay) 
            else:
                print("Max retries reached for this prompt. Returning empty data.")
                return {}

def parse_texts(texts):
    prompts = [INSTRUCTION_PROMPT(text) for text in texts]
    data = []
    for prompt in prompts:
        result = _parse_text_with_retry(prompt)
        data.append(result)
    return data

def parse_text(text):
    prompt = INSTRUCTION_PROMPT(text)
    return _parse_text_with_retry(prompt)