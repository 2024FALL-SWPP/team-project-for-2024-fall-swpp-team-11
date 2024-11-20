
import os
from github import Github
import git
from dotenv import load_dotenv

load_dotenv(override=True)

def pull_repo(repo_url, local_repo_path):
    token = os.getenv("GITHUB_ACCESS_TOKEN")

    if not os.path.exists(local_repo_path):
        git.Repo.clone_from(repo_url.replace('https://', f'https://{token}@'), local_repo_path)
    else:
        repo_git = git.Repo(local_repo_path)
        repo_git.remote().pull()
        
def update_markdown(repo_path, md_name, comment, overwrite=False, idx=None):
    flag = "w" if overwrite else "a"
    
    json_data = comment['json_data']
    with open(os.path.join(repo_path, md_name), flag, encoding='utf-8') as f:
        if flag == "w":
            # write header
            f.write("# Development Logs\n\n")
        
        if idx is not None:
            f.write(f"\n### Log {idx+1}\n\n")
        else:
            f.write(f"\n### Log ({comment['id']})\n\n")
        f.write(f"|구분|내용|\n|---|--|\n")
        f.write(f"|Task ID|{', '.join(json_data['task_id'])}|\n")
        f.write(f"|작업자|{', '.join(json_data['workers'])}|\n")
        f.write(f"|Status|{json_data['status']}|\n")
        f.write(f"|진행일|{json_data['date']}|\n")
        f.write(f"|진행 시간|{json_data['working_time']}|\n")
        f.write(f"|장소|{', '.join(json_data['location'])}|\n")
        f.write(f"|참가자(역할)|{', '.join(json_data['roles'])}|\n")
        f.write(f"|작업 내용|{json_data['work_summary']}|\n")
        f.write(f"|비고|{json_data['notes']}|\n")
        
        if refernce_links := json_data.get("reference_links"):
            f.write(f"|참고 링크|[Drive Link]({comment['share_link']}), ")
            for link in refernce_links:
                if "pull" in link:
                    # pull request
                    pull_no = link.split("/")[-1]
                    f.write(f"[PR#{pull_no}]({link}), ")
                elif "wiki" in link:
                    # wiki page
                    f.write(f"[Wiki]({link}), ")
                else:
                    f.write(f"[{link}]({link}), ")
            f.write("|\n")
        else:
            f.write(f"|참고 링크|[Drive Link]({comment['share_link']})|\n")
        
def push_repo(repo_path, message):
    repo_git = git.Repo(repo_path)
    repo_git.git.add(".")
    repo_git.git.commit("-m", message)
    repo_git.remote().push()