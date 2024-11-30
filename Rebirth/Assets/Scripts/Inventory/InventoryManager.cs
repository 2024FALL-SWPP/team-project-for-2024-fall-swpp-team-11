using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : SingletonManager<InventoryManager>
{
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private InventoryUI inventoryUI;
    private InventoryDataContainer inventoryDataContainer;
    private int inventoryCapcity = 32;

    private static string logPrefix = "[InventoryManager] ";

    protected override void Awake()
    {
        base.Awake();

        SaveManager.save += SaveInventory;
        SaveManager.load += async () => await LoadInventoryAsync();

        inventoryDataContainer = new InventoryDataContainer();
        inventoryUI.SetCapacity(inventoryCapcity);
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
        if (inventoryDataContainer.GetTotalItemCount() > inventoryCapcity) return;

        GameObject inventoryItemObj = Instantiate(inventoryItemPrefab);
        InventoryItem inventoryItem = inventoryItemObj.GetComponent<InventoryItem>();
        inventoryItem.Initialize(itemData);

        inventoryDataContainer.AddItem(inventoryItemObj, itemData.dimension);
        inventoryUI.AddItem(inventoryItemObj, itemData.dimension);
    }

    public void RemoveItem(ItemData itemData)
    {
        GameObject inventoryItemObj = inventoryDataContainer.FindItem(itemData);
        if (inventoryItemObj)
        {
            inventoryDataContainer.RemoveItem(inventoryItemObj, itemData.dimension);
            inventoryUI.RemoveItem(inventoryItemObj);
            Destroy(inventoryItemObj);
        }
    }

    public bool HasItem(ItemData itemData)
    {
        return inventoryDataContainer.HasItem(itemData);
    }

    public void RefreshInventoryUI()
    {
        inventoryUI.RefreshInventoryDisplay();
    }
    #endregion

    #region UI Management

    public void HideInventory()
    {
        inventoryUI.HideInventory();
    }

    public void SortInventory()
    {
        // Sort InventoyDataContainer

        // Iterate InventoryDataContainer

            // From first cell, put InventoryItemObj

            // Check last cell,

        // Empty All the cells after last cell
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
            // !LEGACY
            // inventoryUI.Initialize(inventoryDataContainer);
            // inventoryUI.RefreshInventoryDisplay();
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
