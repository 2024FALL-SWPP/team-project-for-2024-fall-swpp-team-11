using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InventoryManager : SingletonManager<InventoryManager>
{
    private InventoryDataContainer inventoryDataContainer;
    private InventoryUI inventoryUI;
    private int inventoryCapacity = 32;

    #region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();

        SaveManager.save += SaveInventory;
        SaveManager.load += async () => await LoadInventoryAsync();

        inventoryDataContainer = new InventoryDataContainer();
        
        inventoryUI = GetComponent<InventoryUI>();
        inventoryUI.SetCapacity(inventoryCapacity);

        Debug.Log($"Current inventory from dimension : {DimensionManager.Instance.GetCurrentDimension()}");
        inventoryUI.RefreshInventoryDimension();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.ToggleInventory();
        }
    }

    private void OnDestroy()
    {
        SaveManager.save -= SaveInventory;
        SaveManager.load -= async () => await LoadInventoryAsync();
    }
    #endregion

    #region Inventory Data
    public void AddItem(ItemData itemData)
    {
        if (inventoryDataContainer.GetTotalItemCount() >= inventoryCapacity) return;

        inventoryDataContainer.AddItem(itemData, itemData.dimension);
        inventoryUI.AddItem(itemData, itemData.dimension);
    }

    public void RemoveItem(ItemData itemData)
    {
        inventoryDataContainer.RemoveItem(itemData, itemData.dimension);
        inventoryUI.RemoveItem(itemData, itemData.dimension);
    }

    public void RemoveItemByName(string itemName)
    {
        Dimension currentDimension = DimensionManager.Instance.GetCurrentDimension();
        List<ItemData> items = inventoryDataContainer.GetItems(currentDimension);

        // 이름이 itemName과 일치하는 아이템 검색
        ItemData targetItem = items.Find(item => item.itemName == itemName);

        if (targetItem != null)
        {
            // 아이템 삭제
            RemoveItem(targetItem);
            Debug.Log($"{itemName} 아이템이 인벤토리에서 삭제되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{itemName} 아이템이 인벤토리에 없습니다.");
        }
    }

    public bool HasItem(ItemData itemData)
    {
        return inventoryDataContainer.HasItem(itemData, itemData.dimension);
    }

    public void UseItem(ItemData itemData)
    {
        if (itemData is IUsable usableItem)
        {
            usableItem.Use();
            RemoveItem(itemData);
        }
    }

    public bool IsInventoryFull()
    {
        return inventoryDataContainer.GetTotalItemCount() >= inventoryCapacity;
    }


    #endregion

    #region UI Management
    public void HandleSceneChange()
    {
        inventoryUI.HideInventory();
        inventoryUI.HideTooltip();
        inventoryUI.RefreshInventoryDimension();
    }

    public void SortInventory()
    {
        Dimension currentDimension = DimensionManager.Instance.GetCurrentDimension();
        inventoryDataContainer.SortItems(item => item.itemName, currentDimension);
        RedrawUI(currentDimension);
    }

    public void RedrawUI(Dimension dimension)
    {
        List<ItemData> items = inventoryDataContainer.GetItems(dimension);
        
        inventoryUI.RedrawUI(items, dimension);
    }

    public void ShowTooltip(ItemData itemData, Vector2 position)
    {
        inventoryUI.ShowTooltip(itemData, position);
    }

    public void HideTooltip()
    {
        inventoryUI.HideTooltip();
    }
    #endregion

    #region Save Management
    public void SaveInventory()
    {
        DiskSaveSystem.SaveInventoryDataToDisk(inventoryDataContainer);
    }

    public async Task LoadInventoryAsync()
    {
        List<ItemData> loadedItems = await DiskSaveSystem.LoadInventoryDataFromDiskAsync();

        inventoryDataContainer = new InventoryDataContainer();
        foreach (ItemData item in loadedItems)
        {
            inventoryDataContainer.AddItem(item, item.dimension);
        }

        RedrawUI(Dimension.TWO_DIMENSION);
        RedrawUI(Dimension.THREE_DIMENSION);
        inventoryUI.RefreshInventoryDimension();
    }
    #endregion
}