import os
import requests
import pdfkit
from urllib.parse import urlparse, quote, unquote
from dotenv import load_dotenv
import argparse

# Load GitHub token from .env file
load_dotenv(override=True)
GITHUB_TOKEN = os.getenv("GITHUB_ACCESS_TOKEN")

def is_url_encoded(url):
    if '%' in url:
        try:
            decoded_url = unquote(url)
            return quote(decoded_url) == url
        except Exception:
            return False
    return False

def get_markdown_url(wiki_page_url):
    parsed_url = urlparse(wiki_page_url)
    path_parts = parsed_url.path.split("/wiki/")
    
    if len(path_parts) != 2:
        raise ValueError("Invalid wiki page URL format")
    
    repo_path = path_parts[0]
    page_name = path_parts[1]
    
    if not is_url_encoded(page_name):
        encoded_page_name = quote(page_name)
        decoded_page_name = page_name
    else:
        encoded_page_name = page_name
        decoded_page_name = unquote(page_name)
    
    markdown_url = f"https://raw.githubusercontent.com/wiki{repo_path}/{encoded_page_name}.md"
    return markdown_url, decoded_page_name

def fetch_markdown_content(markdown_url):
    response = requests.get(markdown_url)
    if response.status_code == 200:
        return response.text
    else:
        raise Exception("Failed to fetch markdown content")

def convert_markdown_to_html(markdown_content):
    payload = {"text": markdown_content}
    headers = {
        "Accept": "application/vnd.github+json",
        "Authorization": f"Bearer {GITHUB_TOKEN}",
        "X-GitHub-Api-Version": "2022-11-28",
    }

    response = requests.post(
        "https://api.github.com/markdown", json=payload, headers=headers
    )
    if response.status_code == 200:
        return response.text
    else:
        print(response.text)
        raise Exception("Failed to convert markdown to HTML")

def save_html(html_content, filepath):
    html_template = f"""
    <html>
    <head>
    <meta charset="UTF-8">
    <link
        rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/github-markdown-css/5.1.0/github-markdown-light.min.css"
        integrity="sha512-zb2pp+R+czM7GAemdSUQt6jFmr3qCo6ikvBgVU6F5GvwEDR0C2sefFiPEJ9QUpmAKdD5EqDUdNRtbOYnbF/eyQ=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer"
    />
    </head>
    <body>
        <div class="markdown-body">
            {html_content}
        </div>
    </body>
    </html>
    """
    with open(filepath, "w", encoding="utf-8") as file:
        file.write(html_template)

def convert_to_pdf(html_path, pdf_path):
    pdfkit.from_file(html_path, pdf_path)

def main(wiki_page_urls, save_pdf=False, result_dir="results"):
    os.makedirs(result_dir, exist_ok=True)
    for wiki_page_url in wiki_page_urls:
        try:
            markdown_url, page_name = get_markdown_url(wiki_page_url)
            markdown_content = fetch_markdown_content(markdown_url)

            html_content = convert_markdown_to_html(markdown_content)
            html_filename = f"{page_name}.html"
            html_path = os.path.join(result_dir, html_filename)
            save_html(html_content, html_path)
            print(f"Converted {wiki_page_url} to {html_filename}")

            if save_pdf:
                pdf_filename = f"{page_name}.pdf"
                pdf_path = os.path.join(result_dir, pdf_filename)
                convert_to_pdf(html_path, pdf_path)
                print(f"Converted {wiki_page_url} to {pdf_filename}")
        
        except Exception as e:
            print(f"Failed to process {wiki_page_url}: {e}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Convert GitHub Wiki pages to HTML and optionally PDF.")
    parser.add_argument("urls", nargs="+", help="List of GitHub Wiki page URLs to convert.")
    parser.add_argument("--pdf", action="store_true", help="Also convert the HTML to PDF format.")

    args = parser.parse_args()
    main(args.urls, save_pdf=args.pdf)
