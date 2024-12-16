using UnityEngine;

public class ReadableBook : MonoBehaviour, IInteractable
{
    private static string logPrefix = "[ReadableBook] ";

    private Outline outline;
    private InteractableObject2D interactable2D; // 2D용 InteractableObject2D 참조

    public GameObject[] imagesToShow; // 표시할 이미지 배열
    private int currentPage = 0; // 현재 페이지 인덱스
    private bool isImageShown = false;

    private void Awake()
    {
        // 3D용 Outline 컴포넌트 참조 (있을 수도, 없을 수도 있음)
        outline = GetComponent<Outline>();

        // 2D용 InteractableObject2D 컴포넌트 참조 (있을 수도, 없을 수도 있음)
        interactable2D = GetComponent<InteractableObject2D>();

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
                Debug.Log(logPrefix + "Image display hidden.");
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
            Debug.Log(logPrefix + "Image display shown.");
        }
    }

    public void OnFocus()
    {
        if (string.IsNullOrEmpty(gameObject.name))
        {
            Debug.LogWarning(logPrefix + "GameObject name이 설정되지 않았습니다.");
            return;
        }

        Debug.Log(logPrefix + "OnFocus called for ReadableBook: " + gameObject.name);

        if (gameObject.name.EndsWith("2D", System.StringComparison.OrdinalIgnoreCase))
        {
            // 2D ReadableBook의 경우 InteractableObject2D의 OnFocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnFocus();
                Debug.Log(logPrefix + "InteractableObject2D OnFocus called for ReadableBook: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "InteractableObject2D 컴포넌트가 존재하지 않습니다.");
            }
        }
        else
        {
            // 3D ReadableBook의 경우 Outline 활성화
            if (outline != null)
            {
                outline.enabled = true;
                Debug.Log(logPrefix + "3D Outline enabled for ReadableBook: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "Outline 컴포넌트가 존재하지 않습니다.");
            }
        }
    }

    public void OnDefocus()
    {
        if (string.IsNullOrEmpty(gameObject.name))
        {
            Debug.LogWarning(logPrefix + "GameObject name이 설정되지 않았습니다.");
            return;
        }

        Debug.Log(logPrefix + "OnDefocus called for ReadableBook: " + gameObject.name);

        if (gameObject.name.EndsWith("2D", System.StringComparison.OrdinalIgnoreCase))
        {
            // 2D ReadableBook의 경우 InteractableObject2D의 OnDefocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnDefocus();
                Debug.Log(logPrefix + "InteractableObject2D OnDefocus called for ReadableBook: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "InteractableObject2D 컴포넌트가 존재하지 않습니다.");
            }
        }
        else
        {
            // 3D ReadableBook의 경우 Outline 비활성화
            if (outline != null)
            {
                outline.enabled = false;
                Debug.Log(logPrefix + "3D Outline disabled for ReadableBook: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "Outline 컴포넌트가 존재하지 않습니다.");
            }
        }
    }

    private void ShowImage(int index)
    {
        HideAllImages();

        if (index >= 0 && index < imagesToShow.Length)
        {
            imagesToShow[index].SetActive(true);
            Debug.Log(logPrefix + "Showing image at index: " + index);
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
        Debug.Log(logPrefix + "All images hidden.");
    }

    private void ShowNextPage()
    {
        if (currentPage < imagesToShow.Length - 1)
        {
            currentPage++;
            ShowImage(currentPage);
            Debug.Log(logPrefix + "Moved to next page: " + currentPage);
        }
        else
        {
            Debug.Log(logPrefix + "Already at the last page.");
        }
    }

    private void ShowPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowImage(currentPage);
            Debug.Log(logPrefix + "Moved to previous page: " + currentPage);
        }
        else
        {
            Debug.Log(logPrefix + "Already at the first page.");
        }
    }
}
