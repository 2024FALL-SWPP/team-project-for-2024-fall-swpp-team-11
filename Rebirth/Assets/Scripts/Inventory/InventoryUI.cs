using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Transform contentPanel2D;
    [SerializeField] private Transform contentPanel3D;
    [SerializeField] private GameObject tooltip;

    [Header("Grid")]
    [SerializeField] private GameObject gridCellPrefab;
    private List<GridCell> gridCells2D = new List<GridCell>();
    private List<GridCell> gridCells3D = new List<GridCell>();

    private Dictionary<ItemData, int> itemToCellIndex2D = new Dictionary<ItemData, int>();
    private Dictionary<ItemData, int> itemToCellIndex3D = new Dictionary<ItemData, int>();
    private InventoryItemPool itemPool;
    private ItemTooltip itemTooltip;
    private bool isVisible;
    private int inventoryCapacity;

    #region Unity Lifecycle
    private void Start()
    {
        InitializeGridCells();
        itemTooltip = new ItemTooltip(tooltip);
        itemPool = GetComponent<InventoryItemPool>();
    }

    private void OnEnable()
    {
        InventoryEvents.OnItemSwapped += UpdateCellMapping;
    }

    private void OnDisable()
    {
        InventoryEvents.OnItemSwapped -= UpdateCellMapping;
    }

    private void UpdateCellMapping(GridCell originalCell, GridCell targetCell)
    {
        Dictionary<ItemData, int> itemToCellIndex = DimensionManager.Instance.GetCurrentDimension() == Dimension.TWO_DIMENSION
            ? itemToCellIndex2D
            : itemToCellIndex3D;

        // 매핑 업데이트
        ItemData originalItemData = originalCell.inventoryItemObj?.GetComponent<InventoryItem>().itemData;
        ItemData targetItemData = targetCell.inventoryItemObj?.GetComponent<InventoryItem>().itemData;

        if (originalItemData != null)
            itemToCellIndex[originalItemData] = originalCell.index;

        if (targetItemData != null)
            itemToCellIndex[targetItemData] = targetCell.index;

        Debug.Log("Cell mapping updated via event.");
    }
    #endregion

    #region Initialization
    private void InitializeGridCells()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            GridCell cell2D = InstantiateCell(contentPanel2D, i);
            gridCells2D.Add(cell2D);

            GridCell cell3D = InstantiateCell(contentPanel3D, i);
            gridCells3D.Add(cell3D);
        }
    }

    private GridCell InstantiateCell(Transform parent, int index)
    {
        GameObject cellObj = Instantiate(gridCellPrefab, parent);
        GridCell gridCell = cellObj.GetComponent<GridCell>();
        gridCell.index = index;
        return gridCell;
    }
    #endregion

    #region UI Refresh
    public void AddItem(ItemData item, Dimension dimension)
    {
        List<GridCell> gridCells = dimension == Dimension.TWO_DIMENSION ? gridCells2D : gridCells3D;
        Dictionary<ItemData, int> itemToCellIndex = dimension == Dimension.TWO_DIMENSION ? itemToCellIndex2D : itemToCellIndex3D;

        // 빈 셀 찾기
        GridCell emptyCell = gridCells.Find(cell => cell.IsEmpty());
        if (emptyCell == null)
        {
            Debug.LogWarning("No empty cell available.");
            return;
        }

        // 아이템 추가
        GameObject itemObj = itemPool.GetItem();
        emptyCell.AddItem(itemObj);

        InventoryItem inventoryItem = itemObj.GetComponent<InventoryItem>();
        inventoryItem.Initialize(item);

        // 상태 저장
        itemToCellIndex[item] = emptyCell.index;
    }

    public void RemoveItem(ItemData item, Dimension dimension)
    {
        Dictionary<ItemData, int> itemToCellIndex = dimension == Dimension.TWO_DIMENSION ? itemToCellIndex2D : itemToCellIndex3D;

        if (!itemToCellIndex.TryGetValue(item, out int cellIndex))
        {
            Debug.LogWarning("Item not found in UI.");
            return;
        }

        // 셀에서 아이템 제거
        GridCell cell = dimension == Dimension.TWO_DIMENSION ? gridCells2D[cellIndex] : gridCells3D[cellIndex];
        if (cell.inventoryItemObj != null)
        {
            itemPool.ReturnItem(cell.inventoryItemObj);
            cell.RemoveItem();
        }

        // 상태 삭제
        itemToCellIndex.Remove(item);
    }

    public void RedrawUI(List<ItemData> items, Dimension dimension)
    {
        List<GridCell> gridCells = dimension == Dimension.TWO_DIMENSION ? gridCells2D : gridCells3D;
        Dictionary<ItemData, int> itemToCellIndex = dimension == Dimension.TWO_DIMENSION ? itemToCellIndex2D : itemToCellIndex3D;

        itemToCellIndex.Clear();

        foreach (GridCell cell in gridCells)
        {
            if (cell.inventoryItemObj != null)
            {
                itemPool.ReturnItem(cell.inventoryItemObj);
                cell.RemoveItem();
            }
        }

        for (int i = 0; i < items.Count && i < gridCells.Count; i++)
        {
            ItemData item = items[i];
            GameObject itemObj = itemPool.GetItem();
            GridCell cell = gridCells[i];
            cell.AddItem(itemObj);

            InventoryItem inventoryItem = itemObj.GetComponent<InventoryItem>();
            inventoryItem.Initialize(item);

            itemToCellIndex[item] = cell.index;
        }
    }


    public void RefreshInventoryDimension()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.TWO_DIMENSION)
        {
            contentPanel2D.gameObject.SetActive(true);
            contentPanel3D.gameObject.SetActive(false);
        }
        else
        {
            contentPanel2D.gameObject.SetActive(false);
            contentPanel3D.gameObject.SetActive(true);
        }
    }
    #endregion

    #region Toggle Inventory
    public void ToggleInventory()
    {
        if (!isVisible)
            ShowInventory();
        else
            HideInventory();
    }

    public void ShowInventory()
    {
        isVisible = true;
        inventoryUI.gameObject.SetActive(true);
        GameStateManager.Instance.LockView();
    }

    public void HideInventory()
    {
        isVisible = false;
        inventoryUI.gameObject.SetActive(false);
        GameStateManager.Instance.UnlockView();
    }
    #endregion

    #region Tooltip
    public void ShowTooltip(ItemData itemData, Vector2 position)
    {
        itemTooltip.Show();
        itemTooltip.Initialize(itemData);
    }

    public void HideTooltip()
    {
        itemTooltip.Hide();
    }
    #endregion

    #region Configuration
    public void SetCapacity(int capacity)
    {
        inventoryCapacity = capacity;
    }
    #endregion
}
