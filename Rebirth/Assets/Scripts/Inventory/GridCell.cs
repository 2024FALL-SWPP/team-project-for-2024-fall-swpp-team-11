using UnityEngine;

public class GridCell : MonoBehaviour
{
    public GameObject inventoryItemObj {get; private set;}

    public void AddItem(GameObject obj)
    {
        inventoryItemObj = obj;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
    }

    public void RemoveItem()
    {
        inventoryItemObj = null;
    }

    public bool IsEmpty()
    {
        return inventoryItemObj == null;
    }
}
