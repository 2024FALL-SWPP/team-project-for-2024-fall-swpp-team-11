using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    private Item itemData;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 originalPosition;
    private RectTransform viewportRectTransform;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
                
        viewportRectTransform = transform.parent.parent.GetComponent<RectTransform>();
    }
    public void Initialize(Item item)
    {
        itemData = item;
        InitializeInventoryIcon(item);
        InitializeTooltip(item);
    }

    private void InitializeInventoryIcon(Item item)
    {        
        var itemIcon = transform.Find("ItemIcon").GetComponent<Image>();

        itemIcon.sprite = item.icon;
    }

    private void InitializeTooltip(Item item)
    {
        var itemIcon = tooltip.transform.Find("ItemIcon").GetComponent<Image>();
        var itemName = tooltip.transform.Find("ItemName").GetComponent<TMP_Text>();
        var itemDescription = tooltip.transform.Find("ItemDescription").GetComponent<TMP_Text>();

        itemIcon.sprite = item.icon;
        itemName.text = item.itemName;
        itemDescription.text = item.description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // 원래 부모 저장
        originalParent = transform.parent;

        // 부모를 최상위 Canvas로 변경하여 마스크의 영향을 받지 않도록 함
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        transform.SetParent(originalParent, true);

        if (!IsPointerOverViewport(eventData))
        {
            SpawnItemInWorld();
            Inventory.Instance.Remove(itemData);
            Destroy(gameObject);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private bool IsPointerOverViewport(PointerEventData eventData)
    {
        Camera eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        return RectTransformUtility.RectangleContainsScreenPoint(
            viewportRectTransform, 
            eventData.position, 
            eventCamera);
    }

    private void SpawnItemInWorld()
    {
        Vector3 forwardFlat = Camera.main.transform.forward;
        forwardFlat.y = 0;
        forwardFlat = forwardFlat.normalized; 
       
        float spawnDistance = 4f; 
        Vector3 spawnPosition = Camera.main.transform.position + forwardFlat * spawnDistance;
        GameObject worldItem = Instantiate(itemData.prefab, spawnPosition, Quaternion.identity);
       
        worldItem.transform.localScale = Vector3.one;
    }

}
