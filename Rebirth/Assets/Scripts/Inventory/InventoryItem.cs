using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject itemSlot;
    [SerializeField] private GameObject tooltip;
    private ItemData itemData;
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
    
    public void Initialize(ItemData _itemData)
    {
        itemData = _itemData;
        InitializeInventoryIcon(itemData);
        InitializeTooltip(itemData);
    }

    private void InitializeInventoryIcon(ItemData itemData)
    {        
        var itemIcon = itemSlot.transform.Find("ItemIcon").GetComponent<Image>();

        itemIcon.sprite = itemData.icon;
    }

    private void InitializeTooltip(ItemData itemData)
    {
        var itemIcon = tooltip.transform.Find("ItemIcon").GetComponent<Image>();
        var itemName = tooltip.transform.Find("ItemName").GetComponent<TMP_Text>();
        var itemDescription = tooltip.transform.Find("ItemDescription").GetComponent<TMP_Text>();

        itemIcon.sprite = itemData.icon;
        itemName.text = itemData.itemName;
        itemDescription.text = itemData.description;
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
        if (!IsPointerOverViewport(eventData))
            Debug.Log("Out");
        else
            Debug.Log("In");
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

        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        float spawnDistance = cameraController ? cameraController.hDist + 2f : 6f; 
       
        Vector3 spawnPosition = Camera.main.transform.position + forwardFlat * spawnDistance;
        Instantiate(itemData.prefab, spawnPosition, Quaternion.identity);
    }

}
