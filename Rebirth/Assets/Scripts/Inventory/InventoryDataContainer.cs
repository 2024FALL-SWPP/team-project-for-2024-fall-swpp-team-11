using System;
using System.Collections.Generic;

public class InventoryDataContainer
{   
    private List<ItemData> twoDimensionalItems = new List<ItemData>();
    private List<ItemData> threeDimensionalItems = new List<ItemData>();

    public void AddItem(ItemData itemData)
    {
        if (itemData.dimension == Dimension.TWO_DIMENSION)
            twoDimensionalItems.Add(itemData);
        else
            threeDimensionalItems.Add(itemData);
    }

    public void RemoveItem(ItemData itemData)
    {
        if (itemData.dimension == Dimension.TWO_DIMENSION)
            twoDimensionalItems.Remove(itemData);
        else
            threeDimensionalItems.Remove(itemData);
    }

    public List<ItemData> GetItems(Dimension dimension)
    {
        return dimension == Dimension.TWO_DIMENSION ? twoDimensionalItems : threeDimensionalItems;
    }

    public bool HasItem(ItemData itemData, Dimension dimension)
    {
        if (dimension == Dimension.THREE_DIMENSION)
            return threeDimensionalItems.Contains(itemData);
        else
            return twoDimensionalItems.Contains(itemData);
    }

    public int GetTotalItemCount()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
            return threeDimensionalItems.Count;
        else
            return twoDimensionalItems.Count;
    }

    public void SortItems(Func<ItemData, object> keySelector, Dimension dimension, bool ascending = true)
    {
        List<ItemData> items = GetItems(dimension);

        if (ascending)
            items.Sort((a, b) => Comparer<object>.Default.Compare(keySelector(a), keySelector(b)));
        else
            items.Sort((a, b) => Comparer<object>.Default.Compare(keySelector(b), keySelector(a)));
    }
}
