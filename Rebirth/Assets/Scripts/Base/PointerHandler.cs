using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        HandlePointerEnter(eventData);
    } 
    public void OnPointerExit(PointerEventData eventData)
    {
        HandlePointerExit();
    }

    protected virtual void HandlePointerEnter(PointerEventData eventData) { }
    protected virtual void HandlePointerExit() { }
}
