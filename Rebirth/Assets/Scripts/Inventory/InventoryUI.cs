using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform contentPanel2D;
    [SerializeField] private Transform contentPanel3D;
    [SerializeField] private GameObject tooltip;

    [Header("Grid")]
    [SerializeField] private GameObject gridCellPrefab;
    private List<GridCell> gridCells2D = new List<GridCell>();
    private List<GridCell> gridCells3D = new List<GridCell>();
    private ItemTooltip itemTooltip;
    private bool isVisible;
    private int inventoryCapacity;

    private void Start()
    {
        InitializeGridCells();
        itemTooltip = new ItemTooltip(tooltip);
    }

    private void InitializeGridCells()
    {
        GameObject gridCellObj;
        GridCell gridCell;
        for (int cnt = 0; cnt < inventoryCapacity; cnt++)
        {
            gridCellObj = Instantiate(gridCellPrefab, contentPanel2D);
            gridCell = gridCellObj.GetComponent<GridCell>();   
            gridCells2D.Add(gridCell);

            gridCellObj = Instantiate(gridCellPrefab, contentPanel3D);
            gridCell = gridCellObj.GetComponent<GridCell>();
            gridCells3D.Add(gridCell);
        }
    }

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
        inventoryCanvas.gameObject.SetActive(true);
        GameStateManager.Instance.LockView();
        RefreshInventoryDisplay();
    }

    public void HideInventory()
    {
        isVisible = false;
        inventoryCanvas.gameObject.SetActive(false);
        GameStateManager.Instance.UnlockView();
    }
    #endregion

    public void AddItem(GameObject inventoryItemObj, Dimension dimension)
    {
        GridCell emptyCell;
        if (dimension == Dimension.TWO_DIMENSION)
            emptyCell = gridCells2D.Find(cell => cell.IsEmpty());
        else
            emptyCell = gridCells3D.Find(cell => cell.IsEmpty());

        if (emptyCell != null)
        {
            emptyCell.AddItem(inventoryItemObj);
        }
    }

    public void RemoveItem(GameObject inventoryItemObj)
    {
        GridCell parentCell = inventoryItemObj.transform.parent.GetComponent<GridCell>();
        parentCell.RemoveItem();
    }

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

    public void RefreshInventoryDisplay()
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

    public void SetCapacity(int capacity)
    {
        inventoryCapacity = capacity;
    }
}