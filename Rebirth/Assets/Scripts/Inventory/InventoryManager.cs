using UnityEngine;

public class InventoryManager : SingletonManager<InventoryManager>
{
    [SerializeField] private InventoryUI inventoryUI;
    private InventoryData inventoryData;

    private static string logPrefix = "[InventoryManager] ";

    protected override void Awake()
    {
        base.Awake();

        inventoryData = new InventoryData();
        inventoryUI.Initialize(inventoryData);
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
        inventoryData.AddItem(item);
        inventoryUI.RefreshInventoryDisplay();
    }

    public void RemoveItem(ItemData item)
    {
        inventoryData.RemoveItem(item);
        inventoryUI.RefreshInventoryDisplay();
    }

    public bool HasItem(ItemData item)
    {
        return inventoryData.HasItem(item);
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
        SaveSystem.SaveInventoryData(inventoryData);
    }

    public void LoadInventory()
    {
        InventoryData loadedData = SaveSystem.LoadInventoryData();
        if (loadedData != null)
        {
            inventoryData = loadedData;
            inventoryUI.Initialize(inventoryData);
            inventoryUI.RefreshInventoryDisplay();
            Debug.Log("인벤토리가 로드되었습니다.");
        }
    }
    #endregion
}