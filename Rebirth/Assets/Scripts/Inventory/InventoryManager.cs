using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : SingletonManager<InventoryManager>
{
    [SerializeField] private InventoryUI inventoryUI;
    private InventoryDataContainer inventoryDataContainer;

    protected override void Awake()
    {
        base.Awake();

        SaveManager.save += SaveInventory;
        SaveManager.load += async () => await LoadInventoryAsync();

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

    public void RefreshInventoryUI()
    {
        inventoryUI.RefreshInventoryDisplay();
    }
    #endregion

    #region UI Management
    public void ShowInventory()
    {
        inventoryUI.ShowInventory();
    }
    public void HideInventory()
    {
        inventoryUI.HideInventory();
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
        InventoryDataContainer loadedData = await DiskSaveSystem.LoadInventoryDataFromDiskAsync();
        if (loadedData != null)
        {
            inventoryDataContainer = loadedData;
            inventoryUI.Initialize(inventoryDataContainer);
            inventoryUI.RefreshInventoryDisplay();
            Debug.Log("인벤토리가 로드되었습니다.");
        }
    }
    #endregion

    private void OnDestroy()
    {
        SaveManager.save -= SaveInventory;
        SaveManager.load -= async () => await LoadInventoryAsync();
    }
}
