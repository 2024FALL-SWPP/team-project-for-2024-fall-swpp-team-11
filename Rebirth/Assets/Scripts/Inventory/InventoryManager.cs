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
        inventoryUI.Initialize(inventoryDataContainer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.ToggleInventory();
        }
    }

    #region Inventory Data
    public void AddItem(ItemData item)
    {
        inventoryDataContainer.AddItem(item);
        inventoryUI.RefreshInventoryDisplay();
    }

    public void RemoveItem(ItemData item)
    {
        inventoryDataContainer.RemoveItem(item);
        inventoryUI.RefreshInventoryDisplay();
    }

    public bool HasItem(ItemData item)
    {
        return inventoryDataContainer.HasItem(item);
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
            inventoryUI.Initialize(inventoryDataContainer);
            inventoryUI.RefreshInventoryDisplay();
            Debug.Log("인벤토리가 로드되었습니다.");
        }
    }
    #endregion
}