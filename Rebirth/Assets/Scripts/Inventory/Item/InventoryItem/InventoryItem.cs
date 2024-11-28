using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
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
        InitializeInventoryIcon(data);
        itemdragHandler.itemData = data;
        itemPointerHandler.itemData = data;
    }

    private void InitializeInventoryIcon(ItemData itemData)
    {
        itemIcon.sprite = itemData.icon;
    }
}
