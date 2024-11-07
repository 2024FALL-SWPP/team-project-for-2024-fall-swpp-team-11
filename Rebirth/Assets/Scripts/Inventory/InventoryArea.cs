using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isPointerOver;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }
}
