# Dev Log 생성기

## 사용법
1. `.env` 셋업 (`GOOGLE_SERVICE_ACCOUTN_FILE`은 `service-account.json`으로 rename)
2. `id_name.json` 셋업
```json
{
    "U07-------": { # slack ID
        "id": "U07-------",
        "initials": "ABC",
        "name": "홍길동"
    },
    "U08-------": { # slack ID
        "id": "U08-------",
        "initials": "DEF",
        "name": "김철수"
    }
}
```
3. `cd <repo-root>/docs/dev_log`
4. `python main.py`