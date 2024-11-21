using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private GameObject itemSlot;
    private ItemDragHandler dragHandler;
    private ItemPointerHandler itemPointerHandler;

    private void Awake()
    {
        SetupComponents();
    }

    private void SetupComponents()
    {
        dragHandler = GetComponent<ItemDragHandler>();
        itemPointerHandler = GetComponent<ItemPointerHandler>();
    }

    public void Initialize(ItemData data)
    {
        SetupComponents();
        InitializeInventoryIcon(data);
        dragHandler.itemData = data;
        itemPointerHandler.itemData = data;
    }

    private void InitializeInventoryIcon(ItemData itemData)
    {
        var itemIcon = itemSlot.transform.Find("ItemIcon").GetComponent<Image>();
        itemIcon.sprite = itemData.icon;
    }
}
