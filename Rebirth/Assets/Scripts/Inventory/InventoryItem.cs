using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item itemData;
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
                
        viewportRectTransform = transform.parent.parent.GetComponent<RectTransform>(); // Viewport는 Content의 부모
    }
    public void InitializeItem(Item item)
    {
        itemData = item;
        var itemName = transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemIcon = transform.Find("ItemIcon").GetComponent<Image>();

        itemName.text = item.itemName;
        itemIcon.sprite = item.icon;
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
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        GameObject worldItem = Instantiate(itemData.prefab, spawnPosition, Quaternion.identity);

        GameObject worldItemsParent = GameObject.Find("WorldItemsParent");
        if (worldItemsParent != null)
        {
            worldItem.transform.SetParent(worldItemsParent.transform);
        }
        worldItem.transform.localScale = Vector3.one;
    }
}
