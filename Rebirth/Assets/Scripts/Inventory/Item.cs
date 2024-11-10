using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Interact()
    {
        if (itemData)
        {
            Inventory.Instance.Add(itemData);
        }
        
        Destroy(gameObject);
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
}
