using System.Collections.Generic;

public class InventoryDataContainer
{   
    public IReadOnlyList<ItemData> TwoDimensionalItems => twoDimensionalItems;
    public IReadOnlyList<ItemData> ThreeDimensionalItems => threeDimensionalItems;

    private List<ItemData> twoDimensionalItems = new List<ItemData>();
    private List<ItemData> threeDimensionalItems = new List<ItemData>();

    public InventoryDataContainer(List<ItemData> saved2DItems, List<ItemData> saved3DItems)
    {
        twoDimensionalItems = new List<ItemData>(saved2DItems);
        threeDimensionalItems = new List<ItemData>(saved3DItems);
    }

    public InventoryDataContainer()
    {
        twoDimensionalItems = new List<ItemData>();
        threeDimensionalItems = new List<ItemData>();
    }

    public void AddItem(ItemData item)
    {
        if (item.dimension == Dimension.THREE_DIMENSION)
            threeDimensionalItems.Add(item);
        else
            twoDimensionalItems.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        if (item.dimension == Dimension.THREE_DIMENSION)
            threeDimensionalItems.Remove(item);
        else
            twoDimensionalItems.Remove(item);
    }

    public bool HasItem(ItemData item)
    {
        if (item.dimension == Dimension.THREE_DIMENSION)
            return threeDimensionalItems.Contains(item);
        else
            return twoDimensionalItems.Contains(item);
    }
}
