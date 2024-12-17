using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private static string logPrefix = "[InventoryUI] ";

    [Header("UI References")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Transform contentPanel2D;
    [SerializeField] private Transform contentPanel3D;
    [SerializeField] private GameObject tooltip;

    [SerializeField] private TMP_Text inventoryTitleText;

    [Header("Grid")]
    [SerializeField] private GameObject gridCellPrefab;
    private List<GridCell> gridCells2D = new List<GridCell>();
    private List<GridCell> gridCells3D = new List<GridCell>();

    private InventoryItemPool itemPool;
    private ItemTooltip itemTooltip;
    private int inventoryCapacity;

    #region Unity Lifecycle
    private void Start()
    {
        InitializeGridCells();
        itemTooltip = new ItemTooltip(tooltip);
        itemPool = GetComponent<InventoryItemPool>();
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
    public void AddItem(ItemData item)
    {
        List<GridCell> gridCells = item.dimension == Dimension.TWO_DIMENSION ? gridCells2D : gridCells3D;

        // 빈 셀 찾기
        GridCell emptyCell = gridCells.Find(cell => cell.IsEmpty());
        if (emptyCell == null)
            return;

        // 아이템 추가
        GameObject itemObj = itemPool.GetItem();
        emptyCell.AddItem(itemObj);

        InventoryItem inventoryItem = itemObj.GetComponent<InventoryItem>();
        inventoryItem.Initialize(item);
    }

    public void RemoveItem(ItemData item)
    {
        List<GridCell> gridCells = item.dimension == Dimension.TWO_DIMENSION ? gridCells2D : gridCells3D;

        foreach (GridCell cell in gridCells)
        {
            if (cell.inventoryItemObj != null)
            {
                InventoryItem inventoryItem = cell.inventoryItemObj.GetComponent<InventoryItem>();
                if (inventoryItem != null && inventoryItem.itemData.name == item.name)
                {
                    itemPool.ReturnItem(cell.inventoryItemObj);
                    cell.RemoveItem();
                    return;
                }
            }
        }
    }

    public void RemoveItem(Dimension dimension, GameObject inventoryItemObj)
    {
        List<GridCell> gridCells = dimension == Dimension.TWO_DIMENSION ? gridCells2D : gridCells3D;

        foreach (GridCell cell in gridCells)
        {
            if (cell.inventoryItemObj != null)
            {
                if (cell.inventoryItemObj == inventoryItemObj)
                {
                    itemPool.ReturnItem(cell.inventoryItemObj);
                    cell.RemoveItem();
                    return;
                }
            }
        }
    }

    public void RedrawUI(List<ItemData> items, Dimension dimension)
    {
        List<GridCell> gridCells = dimension == Dimension.TWO_DIMENSION ? gridCells2D : gridCells3D;

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
        }
    }

    public void RefreshInventoryDimension()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.TWO_DIMENSION)
        {
            Debug.Log(logPrefix + "Current Inventory Set to 2D");
            contentPanel2D.gameObject.SetActive(true);
            contentPanel3D.gameObject.SetActive(false);
            inventoryTitleText.text = "Inventory (2D)";
        }
        else
        {
            Debug.Log(logPrefix + "Current Inventory Set to 3D");
            contentPanel2D.gameObject.SetActive(false);
            contentPanel3D.gameObject.SetActive(true);
            inventoryTitleText.text = "Inventory (3D)";
        }
    }
    #endregion

    #region Toggle Inventory
    public void ToggleInventory()
    {
        if (!inventoryUI.activeSelf)
            ShowInventory();
        else
            HideInventory();
    }

    public void ShowInventory()
    {
        inventoryUI.gameObject.SetActive(true);
        UIManager.Instance.AddToUIStack(inventoryUI);
        GameStateManager.Instance.LockView();
    }

    public void HideInventory()
    {
        inventoryUI.gameObject.SetActive(false);
        UIManager.Instance.RemoveFromUIStack(inventoryUI);
        GameStateManager.Instance.UnlockView();
    }
    #endregion

    #region Tooltip
    public void ShowTooltip(ItemData itemData, Vector2 position)
    {
        Debug.Log(logPrefix + "Show Tooltip / itemData: " + itemData + ", position: " + position);
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
