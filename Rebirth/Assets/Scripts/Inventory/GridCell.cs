using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int index;
    public GameObject inventoryItemObj { get; private set; }

    public void AddItem(GameObject obj)
    {
        inventoryItemObj = obj;
        obj.transform.SetParent(transform);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector3.zero;
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
