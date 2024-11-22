using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject tooltip;
    private bool isVisible = false;
    private ItemTooltip itemTooltip;
    private InventoryDataContainer inventoryDataContainer;

    public void Initialize(InventoryDataContainer dataContainer)
    {
        inventoryDataContainer = dataContainer;
        itemTooltip = new ItemTooltip(tooltip);
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
        inventoryUI.SetActive(true);
        GameStateManager.Instance.LockView();
        RefreshInventoryDisplay();
    }

    public void HideInventory()
    {
        isVisible = false;
        inventoryUI.SetActive(false);
        GameStateManager.Instance.UnlockView();
    }
    #endregion

    #region Draw Inventory UI
    public void RefreshInventoryDisplay()
    {
        ClearInventoryUI();
        RegenerateInventoryUI();
        HideTooltip(); // hide tooltip when inventory is refreshed
    }

    private void ClearInventoryUI()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void RegenerateInventoryUI()
    {
        IReadOnlyList<ItemData> itemDatas;
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.TWO_DIMENSION)
            itemDatas = inventoryDataContainer.TwoDimensionalItems;
        else
            itemDatas = inventoryDataContainer.ThreeDimensionalItems;

        foreach (var itemData in itemDatas)
        {
            GameObject inventoryItem = Instantiate(inventoryItemPrefab, contentPanel);
            inventoryItem.GetComponent<InventoryItem>().Initialize(itemData);
        }
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
}