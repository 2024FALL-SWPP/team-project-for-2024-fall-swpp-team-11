using UnityEngine;

public class ReadableBook : MonoBehaviour, IInteractable
{
    private Outline outline;
    private Outline2D outline2D;

    public GameObject[] imagesToShow; // 표시할 이미지 배열
    private int currentPage = 0; // 현재 페이지 인덱스
    private bool isImageShown = false;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline2D = GetComponent<Outline2D>();
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
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            if (!outline) return;
            outline.enabled = true;
        }
        else
        {
            if (!outline2D) return;
            outline2D.SetOutline();
        }
    }

    public void OnDefocus()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            if (!outline) return;
            outline.enabled = false;
        }
        else
        {
            if (!outline2D) return;
            outline2D.UnsetOutline();
        }
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
