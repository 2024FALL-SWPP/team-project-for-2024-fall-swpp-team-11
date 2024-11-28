using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject tooltip;

    [Header("Grid")]
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private int maxRow = 10;
    [SerializeField] private int maxCol = 4;
    private List<GridCell> gridCells = new List<GridCell>();

    private ItemTooltip itemTooltip;
    private bool isVisible;

    private void Start()
    {
        InitializeGridCells();
        itemTooltip = new ItemTooltip(tooltip);
    }

    private void InitializeGridCells()
    {
        for (int row = 0; row < maxRow; row++) // 10 rows
        {
            for (int col = 0; col < maxCol; col++) // 4 columns
            {
                GameObject gridCellObj = Instantiate(gridCellPrefab, contentPanel);

                // GridCell 컴포넌트 설정
                GridCell gridCell = gridCellObj.GetComponent<GridCell>();
                gridCell.row = row;
                gridCell.column = col;

                gridCells.Add(gridCell);
            }
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

    public void AddItem(ItemData itemData)
    {
        GridCell emptyCell = gridCells.Find(cell => cell.IsEmpty());
        if (emptyCell != null)
        {
            GameObject inventoryItemObj = Instantiate(inventoryItemPrefab);
            InventoryItem inventoryItem = inventoryItemObj.GetComponent<InventoryItem>();
            inventoryItem.Initialize(itemData);

            emptyCell.AddItem(inventoryItemObj);
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        // GridCell targetCell = gridCells.Find(cell => cell.inventoryItemObj.);
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
}