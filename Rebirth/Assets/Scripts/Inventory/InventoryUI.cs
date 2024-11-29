using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject tooltip;

    [Header("Grid")]
    [SerializeField] private GameObject gridCellPrefab;
    private List<GridCell> gridCells = new List<GridCell>();
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
        for (int cnt = 0; cnt < inventoryCapacity; cnt++)
        {
            GameObject gridCellObj = Instantiate(gridCellPrefab, contentPanel);
            GridCell gridCell = gridCellObj.GetComponent<GridCell>();
            gridCells.Add(gridCell);
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
        // RefreshInventoryDisplay();
    }

    public void HideInventory()
    {
        isVisible = false;
        inventoryCanvas.gameObject.SetActive(false);
        GameStateManager.Instance.UnlockView();
    }
    #endregion

    public void AddItem(GameObject inventoryItemObj)
    {
        GridCell emptyCell = gridCells.Find(cell => cell.IsEmpty());
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

    public void SetCapacity(int capacity)
    {
        inventoryCapacity = capacity;
    }
}