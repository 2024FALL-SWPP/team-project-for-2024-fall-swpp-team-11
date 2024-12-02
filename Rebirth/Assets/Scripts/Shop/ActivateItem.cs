using UnityEngine;

public class ActivateItem : MonoBehaviour
{
    private bool isActive = false; // Track if the item is active

    private void Start()
    {
        // Ensure the item is deactivated at the start
        ResetItem();
    }

    // Call this method to activate the item
    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            gameObject.SetActive(true);
            Debug.Log($"{gameObject.name} has been activated.");
        }
    }

    // Call this method to deactivate the item
    public void Deactivate()
    {
        if (isActive)
        {
            isActive = false;
            gameObject.SetActive(false);
            Debug.Log($"{gameObject.name} has been deactivated.");
        }
    }

    // Use this to reset item state if needed
    public void ResetItem()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
