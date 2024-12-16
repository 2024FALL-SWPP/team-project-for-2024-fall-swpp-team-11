using UnityEngine;

public class ShopInteractable : MonoBehaviour, IInteractable
{
    private static string logPrefix = "[ShopInteractable] ";

    private Outline outline;
    private InteractableObject2D interactable2D; // 2D용 InteractableObject2D 참조

    private void Awake()
    {
        // 3D용 Outline 컴포넌트 참조 (있을 수도, 없을 수도 있음)
        outline = GetComponent<Outline>();

        // 2D용 InteractableObject2D 컴포넌트 참조 (있을 수도, 없을 수도 있음)
        interactable2D = GetComponent<InteractableObject2D>();
    }

    public void Interact()
    {
        ShopManager.Instance.ToggleShopUI();
    }

    public void OnFocus()
    {
        if (string.IsNullOrEmpty(gameObject.name))
        {
            Debug.LogWarning(logPrefix + "GameObject name이 설정되지 않았습니다.");
            return;
        }

        Debug.Log(logPrefix + "OnFocus called for Shop: " + gameObject.name);

        if (gameObject.name.EndsWith("2D", System.StringComparison.OrdinalIgnoreCase))
        {
            // 2D Shop의 경우 InteractableObject2D의 OnFocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnFocus();
                Debug.Log(logPrefix + "InteractableObject2D OnFocus called for Shop: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "InteractableObject2D 컴포넌트가 존재하지 않습니다.");
            }
        }
        else
        {
            // 3D Shop의 경우 Outline 활성화
            if (outline != null)
            {
                outline.enabled = true;
                Debug.Log(logPrefix + "3D Outline enabled for Shop: " + gameObject.name);
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

        Debug.Log(logPrefix + "OnDefocus called for Shop: " + gameObject.name);

        if (gameObject.name.EndsWith("2D", System.StringComparison.OrdinalIgnoreCase))
        {
            // 2D Shop의 경우 InteractableObject2D의 OnDefocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnDefocus();
                Debug.Log(logPrefix + "InteractableObject2D OnDefocus called for Shop: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "InteractableObject2D 컴포넌트가 존재하지 않습니다.");
            }
        }
        else
        {
            // 3D Shop의 경우 Outline 비활성화
            if (outline != null)
            {
                outline.enabled = false;
                Debug.Log(logPrefix + "3D Outline disabled for Shop: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning(logPrefix + "Outline 컴포넌트가 존재하지 않습니다.");
            }
        }
    }
}
