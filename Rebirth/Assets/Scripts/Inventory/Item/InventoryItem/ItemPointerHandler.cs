using UnityEngine.EventSystems;

public class ItemPointerHandler : PointerHandler
{
    public ItemData itemData;
    protected override void HandlePointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.ShowTooltip(itemData, eventData.position);
    }

    protected override void HandlePointerExit()
    {
        InventoryManager.Instance.HideTooltip();
    }
}
