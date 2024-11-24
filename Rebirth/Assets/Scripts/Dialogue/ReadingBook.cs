using UnityEngine;

public class ReadingBook : MonoBehaviour, IInteractable
{
    private Outline outline;

    public GameObject[] imagesToShow; // 표시할 이미지 배열
    private int currentPage = 0; // 현재 페이지 인덱스
    private bool isImageShown = false;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        HideAllImages();
    }

    private void Update()
    {
        if (isImageShown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideAllImages();
                Time.timeScale = 1f;
                isImageShown = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ShowPreviousPage();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShowNextPage();
            }
        }
    }

    public void Interact()
    {
        if (imagesToShow.Length > 0)
        {
            currentPage = 0;
            ShowImage(currentPage);
            isImageShown = true;
            Time.timeScale = 0f;
        }
    }

    public void OnFocus()
    {
        if (!outline) return;

        outline.enabled = true;
    }

    public void OnDefocus()
    {
        if (!outline) return;

        outline.enabled = false;
    }

    private void ShowImage(int index)
    {
        HideAllImages();

        if (index >= 0 && index < imagesToShow.Length)
        {
            imagesToShow[index].SetActive(true);
        }
    }

    private void HideAllImages()
    {
        foreach (var image in imagesToShow)
        {
            if (image != null)
            {
                image.SetActive(false);
            }
        }
    }

    private void ShowNextPage()
    {
        if (currentPage < imagesToShow.Length - 1)
        {
            currentPage++;
            ShowImage(currentPage);
        }
    }

    private void ShowPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowImage(currentPage);
        }
    }
}
