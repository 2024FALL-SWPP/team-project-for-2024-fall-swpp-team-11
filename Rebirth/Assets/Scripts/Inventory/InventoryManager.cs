using UnityEngine;

public class InventoryManager : SingletonManager<InventoryManager>
{
    [SerializeField] private InventoryUI inventoryUI;
    private InventoryDataContainer inventoryDataContainer;

    private static string logPrefix = "[InventoryManager] ";

    protected override void Awake()
    {
        base.Awake();

        inventoryDataContainer = new InventoryDataContainer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.ToggleInventory();
        }
    }

    #region Inventory Data
    public void AddItem(ItemData itemData)
    {
        inventoryDataContainer.AddItem(itemData);
        inventoryUI.AddItem(itemData);
    }

    public void RemoveItem(ItemData itemData)
    {
        inventoryDataContainer.RemoveItem(itemData);
        inventoryUI.RemoveItem(itemData);
    }

    public bool HasItem(ItemData itemData)
    {
        return inventoryDataContainer.HasItem(itemData);
    }

    public void RefreshInventoryUI()
    {
        // inventoryUI.RefreshInventoryDisplay();
    }
    #endregion

    #region UI Management
    public void ShowTooltip(ItemData itemData, Vector2 position)
    {
        Debug.Log(logPrefix + "show tool tip");
        inventoryUI.ShowTooltip(itemData, position);
    }

    public void HideTooltip()
    {
        Debug.Log(logPrefix + "Hide tip");
        inventoryUI.HideTooltip();
    }
    #endregion

    #region Save Management
    public void SaveInventory()
    {
        SaveSystem.SaveInventoryData(inventoryDataContainer);
    }

    public void LoadInventory()
    {
        InventoryDataContainer loadedData = SaveSystem.LoadInventoryData();
        if (loadedData != null)
        {
            inventoryDataContainer = loadedData;
            // !LEGACY
            // inventoryUI.Initialize(inventoryDataContainer);
            // inventoryUI.RefreshInventoryDisplay();
            Debug.Log("인벤토리가 로드되었습니다.");
        }
    }
    #endregion
}