using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPool : MonoBehaviour
{
    [SerializeField] private GameObject inventoryItemPrefab;
    private Queue<GameObject> itemPool = new Queue<GameObject>();

    public GameObject GetItem()
    {
        if (itemPool.Count > 0)
        {
            GameObject item = itemPool.Dequeue();
            item.SetActive(true);
            return item;
        }
        return Instantiate(inventoryItemPrefab);
    }

    public void ReturnItem(GameObject item)
    {
        item.SetActive(false);
        itemPool.Enqueue(item);
    }
}
