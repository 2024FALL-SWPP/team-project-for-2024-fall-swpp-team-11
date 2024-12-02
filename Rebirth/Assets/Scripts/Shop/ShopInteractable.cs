using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ShopInteractable : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameObject shopUI;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }


    public void Interact()
    {
      
        ShopManager.Instance.ToggleShopUI();
    }

    public void OnFocus()
    {
        outline.enabled = true;
    }

    public void OnDefocus()
    {
        outline.enabled = false;
        // shopUI.SetActive(false);
        GameStateManager.Instance.UnlockView();
    }
}
