using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject itemSlot;
    private ItemData itemData;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private RectTransform viewportRectTransform;
    private ItemDragHandler dragHandler;

    private void Awake()
    {
        SetupComponents();
    }

    private void SetupComponents()
    {
        rectTransform ??= GetComponent<RectTransform>();
        canvasGroup ??= GetComponent<CanvasGroup>();
        canvas ??= GetComponentInParent<Canvas>();
        viewportRectTransform ??= transform.parent.parent.GetComponent<RectTransform>();
    }

    public void Initialize(ItemData data)
    {
        itemData = data;
        SetupComponents();
        InitializeInventoryIcon(itemData);
        dragHandler = new ItemDragHandler(
            rectTransform,
            canvasGroup,
            canvas,
            rectTransform.anchoredPosition,
            transform.parent
        );
    }

    private void InitializeInventoryIcon(ItemData itemData)
    {
        var itemIcon = itemSlot.transform.Find("ItemIcon").GetComponent<Image>();
        itemIcon.sprite = itemData.icon;
    }

    #region IItemInteraction Implementation
    public void HandlePointerEnter(PointerEventData eventData) => InventoryManager.Instance.ShowTooltip(itemData, eventData.position);
    public void HandlePointerExit() => InventoryManager.Instance.HideTooltip();
    public void HandleDragStart(PointerEventData eventData) => dragHandler.StartDrag();
    public void HandleDrag(PointerEventData eventData) => dragHandler.Drag(eventData);
    public void HandleDragEnd(PointerEventData eventData)
    {
        if (!IsPointerOverViewport(eventData))
        {
            SpawnItemInWorld();
            InventoryManager.Instance.RemoveItem(itemData);
            Destroy(gameObject);
        }
        else
        {
            dragHandler.EndDrag();
        }
    }
    #endregion

    #region Event System Callbacks
    public void OnPointerEnter(PointerEventData eventData) => HandlePointerEnter(eventData);
    public void OnPointerExit(PointerEventData eventData) => HandlePointerExit();
    public void OnBeginDrag(PointerEventData eventData) => HandleDragStart(eventData);
    public void OnDrag(PointerEventData eventData) => HandleDrag(eventData);
    public void OnEndDrag(PointerEventData eventData) => HandleDragEnd(eventData);
    #endregion

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
        Vector3 spawnPosition = CalculateSpawnPosition();
        Instantiate(itemData.prefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 forwardFlat = Camera.main.transform.forward;
        forwardFlat.y = 0;
        forwardFlat = forwardFlat.normalized;

        var cameraController = Camera.main.GetComponent<CameraController3D>();
        float spawnDistance = cameraController ? cameraController.hDist + 2f : 6f;

        return Camera.main.transform.position + forwardFlat * spawnDistance;
    }

}
