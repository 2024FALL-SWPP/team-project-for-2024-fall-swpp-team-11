using System.Collections.Generic;

public class InventoryData
{   
    public IReadOnlyList<ItemData> Items => items;
    private List<ItemData> items = new List<ItemData>();

    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }
}
