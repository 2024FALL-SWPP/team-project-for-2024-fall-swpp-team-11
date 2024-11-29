using System.Collections.Generic;
using UnityEngine;

public class InventoryDataContainer
{   
    public IReadOnlyList<GameObject> TwoDimensionalItems => twoDimensionalItems;
    public IReadOnlyList<GameObject> ThreeDimensionalItems => threeDimensionalItems;

    private List<GameObject> twoDimensionalItems = new List<GameObject>();
    private List<GameObject> threeDimensionalItems = new List<GameObject>();

    public InventoryDataContainer(List<GameObject> saved2DItems, List<GameObject> saved3DItems)
    {
        twoDimensionalItems = new List<GameObject>(saved2DItems);
        threeDimensionalItems = new List<GameObject>(saved3DItems);
    }

    public InventoryDataContainer()
    {
        twoDimensionalItems = new List<GameObject>();
        threeDimensionalItems = new List<GameObject>();
    }

    public void AddItem(GameObject inventoryItemObj, Dimension dimension)
    {
        if (dimension == Dimension.THREE_DIMENSION)
            threeDimensionalItems.Add(inventoryItemObj);
        else
            twoDimensionalItems.Add(inventoryItemObj);
    }

    public void RemoveItem(GameObject inventoryItemObj, Dimension dimension)
    {
        if (dimension == Dimension.THREE_DIMENSION)
            threeDimensionalItems.Remove(inventoryItemObj);
        else
            twoDimensionalItems.Remove(inventoryItemObj);
    }

    public GameObject FindItem(ItemData item)
    {
        if (item.dimension == Dimension.THREE_DIMENSION)
            return threeDimensionalItems.Find(obj => obj.GetComponent<InventoryItem>()?.itemData == item);
        else
            return twoDimensionalItems.Find(obj => obj.GetComponent<InventoryItem>()?.itemData == item);
    }

    public bool HasItem(ItemData item)
    {
        return FindItem(item) != null;
    }

    public int GetTotalItemCount()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
            return threeDimensionalItems.Count;
        else
            return twoDimensionalItems.Count;
    }
}
