using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;
    private Image itemIcon;
    private ItemDragHandler itemdragHandler;
    private ItemPointerHandler itemPointerHandler;

    private void Awake()
    {
        itemIcon = GetComponent<Image>();
        SetupComponents();
    }

    private void SetupComponents()
    {
        itemdragHandler = GetComponent<ItemDragHandler>();
        itemPointerHandler = GetComponent<ItemPointerHandler>();
    }

    public void Initialize(ItemData data)
    {
        SetupComponents();
        itemData = data;
        itemIcon.sprite = data.icon;
        itemdragHandler.itemData = data;
        itemPointerHandler.itemData = data;
    }
}
