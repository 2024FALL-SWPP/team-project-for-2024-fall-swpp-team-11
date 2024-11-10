using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<ItemData> ItemDatas = new List<ItemData>();
    public Transform InventoryContent;
    public GameObject InventoryItem;

    private void Awake()
    { 
        Instance = this;
    }

    public void Add(ItemData itemData)
    {
        ItemDatas.Add(itemData);
        ListItems();
    }

    public void Remove(ItemData itemData)
    {
        ItemDatas.Remove(itemData);
        ListItems();
    }

    public void ListItems()
    {
        foreach (Transform itemData in InventoryContent)
        {
            Destroy(itemData.gameObject);
        }
        
        foreach (var itemData in ItemDatas)
        {
            GameObject obj = Instantiate(InventoryItem, InventoryContent);
            var inventoryItem = obj.GetComponent<InventoryItem>();
            inventoryItem.Initialize(itemData);
        }
    }
}
