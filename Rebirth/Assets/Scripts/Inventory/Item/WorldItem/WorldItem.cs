using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
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
            InventoryManager.Instance.AddItem(itemData);
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
