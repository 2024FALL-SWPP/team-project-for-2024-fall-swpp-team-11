import os, json, time
from dotenv import load_dotenv
from openai import OpenAI

load_dotenv(override=True)

INSTRUCTION_PROMPT = lambda text: {
    "role": "system",
    "content": f"""
Extract the following fields from the input and return ad JSON.
Do not include any additional information. Just return the fields as JSON.
You should return the JSON just as it is in the input.
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
- reference_links (list of strings. You should parse correct URLs from the input)

Input Format (There might be some subtle differences in the input format):
[당일 업무 보고(MM월 DD일) - OOO]
진행한 일 - (task id)
상태 - (status)
진행한 시간 - HH:MM ~ HH:MM (N시간 N분)
장소 - (ex. 관정 425호)
참가자(역할) - (roles)
업무 내용 요약- (업무 내용)
업무 관련 자료(증빙 사진 등)-
ex) 블렌더의 경우 3장 이상 캡쳐한 사진을, 코딩을 한 경우 github PR 링크 또는 commit을, 다른 업무의 경우 증빙할 수 있는 어떠한 자료(줌 스샷, 단체 사진 등등)
기타 특이사항 -
ex) 어떠어떠한 부분이 어렵습니다.. 도움이 필요합니다. 등 코드 리뷰가 필요한 경우, PM등을 멘션.

Input:
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