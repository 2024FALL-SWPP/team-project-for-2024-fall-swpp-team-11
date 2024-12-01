using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ShopInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject shopUI;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Start()
    {
        shopUI.SetActive(false); // Disable the UI at the start
    }

    public void Interact()
    {
        // Call the ShopUI's ToggleShopUI method to manage the shop
        ShopUI shopUIScript = shopUI.GetComponent<ShopUI>();
        shopUIScript.ToggleShopUI();
    }

    public void OnFocus()
    {
        outline.enabled = true; // Enable outline when the shop is in focus
    }

    public void OnDefocus()
    {
        outline.enabled = false; // Disable outline when the shop is out of range
        shopUI.SetActive(false); // Ensure UI is hidden when defocused
        GameStateManager.Instance.UnlockView(); // Unlock camera
    }
}
