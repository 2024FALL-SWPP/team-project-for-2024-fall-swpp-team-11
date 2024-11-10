using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> Items = new List<Item>();
    public Transform InventoryContent;
    public GameObject InventoryItem;

    private void Awake()
    { 
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
        ListItems();
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        ListItems();
    }

    public void ListItems()
    {
        foreach (Transform item in InventoryContent)
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, InventoryContent);
            var inventoryItem = obj.GetComponent<InventoryItem>();
            inventoryItem.Initialize(item);
        }
    }
}
