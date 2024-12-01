using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        HandlePointerEnter(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    } 

    public void OnPointerExit(PointerEventData eventData)
    {
        HandlePointerExit();
    }

    protected virtual void HandlePointerEnter(PointerEventData eventData) { }
    protected virtual void HandlePointerExit() { }
    protected virtual void HandlePointerClick(PointerEventData eventData) { }
}
