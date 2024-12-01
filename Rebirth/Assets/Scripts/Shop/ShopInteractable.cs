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
        // Set the shop name based on this object's name
        ShopUI shopUIScript = shopUI.GetComponent<ShopUI>();
        shopUIScript.SetShopName(gameObject.name);
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
