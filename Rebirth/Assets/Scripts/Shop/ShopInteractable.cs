using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject shopUI;
    private Outline outline;

    private void Awake()
    {
        // Get the Outline component attached to the shop counter
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Disable the outline initially
        }
    }

    private void Start()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false); // Disable the UI at the start of the game
        }
        else
        {
            Debug.LogError("shopUI is not assigned in the Inspector!");
        }
    }

    public void Interact()
    {
        if (shopUI != null)
        {
            // Toggle the shop UI's active state
            bool isShopUIActive = !shopUI.activeSelf;
            shopUI.SetActive(isShopUIActive);
            Debug.Log("shopUI active state toggled: " + isShopUIActive);

            // Lock or unlock the camera based on the UI state
            if (isShopUIActive)
            {
                GameStateManager.Instance.LockView(); // Lock camera movement
            }
            else
            {
                GameStateManager.Instance.UnlockView(); // Unlock camera movement
            }
        }
        else
        {
            Debug.LogError("shopUI is not assigned!");
        }
    }

    public void OnFocus()
    {
        if (outline != null)
        {
            outline.enabled = true; // Enable the outline when the shop is in focus
        }
        Debug.Log("Shop in range!");
    }

    public void OnDefocus()
    {
        if (outline != null)
        {
            outline.enabled = false; // Disable the outline when the shop is out of range
        }

        if (shopUI != null)
        {
            shopUI.SetActive(false); // Ensure the shop UI is hidden when defocused
            GameStateManager.Instance.UnlockView(); // Unlock camera when defocused
        }
        Debug.Log("Shop out of range!");
    }
}
