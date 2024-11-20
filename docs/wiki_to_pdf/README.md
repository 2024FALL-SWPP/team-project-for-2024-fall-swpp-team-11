# Wiki Page to PDF

## Usage
1. `pip install -r requirements.txt`
2. make `.env`, insert `GITHUB_ACCESS_TOKEN`
3. `python convert.py <wiki-page-url-1> <wiki-page-url-2> ...`

### Save PDF
The default behavior of this code is saving into HTML. If you also want to convert it as PDF, you need to install `wkhtmltopdf` first and add `--pdf` option
- Install `wkhtmltopdf` (Windows: [Install Page](https://wkhtmltopdf.org/downloads.html), Linux: `sudo apt-get install wkhtmltopdf`, MacOS: `brew install wkhtmltopdf`)
- Set environment variables if needed
- `cd <repo-root>/docs/wiki_to_pdf`
- `python convert.py --pdf <wiki-page-url-1> <wiki-page-url-2> ...`

## Description

### Get Markdown source

Get raw markdown url using your wiki page path and fetch it.

### Convert to Github-favored HTML

We use github API. 

Reqeust for the corresponding HTML using below:
```
curl -L \
  -X POST \
  -H "Accept: application/vnd.github+json" \
  -H "Authorization: Bearer <YOUR-TOKEN>" \
  -H "X-GitHub-Api-Version: 2022-11-28" \
  https://api.github.com/markdown \
  -d '{"text":"Hello **world**"}'
```
- Github Token can be accessed with `.env` file, using key `GITHUB_ACCESS_TOKEN`
- The value of `"text"` in json body is the raw markdown content

### Reconstruct HTML
The response HTML is merely content that can be inserted into `<div class="markdown-body">` element. We need to do it manually. The template is 

```HTML
<html>
<head>
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
        <!-- Insert converted content here -->
    </div>
</body>
</html>
```

which contains both `markdown-body` elmeent and github-markdown-css file. 

### Save HTML

We now save the HTML. The name of the file is same as wiki page. If wiki page markdown URL is `github.com/.../wiki/page-name-example.md`, then the name of the wiki page is `page-name-example`, which is the last one of URL. So, the name of the HTML is `page-name-example.HTML`