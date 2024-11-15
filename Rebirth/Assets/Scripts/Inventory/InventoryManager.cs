using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private InventoryUI inventoryUI;
    private InventoryData inventoryData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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
    #endregion

    #region UI Management
    public void ShowTooltip(ItemData itemData, Vector2 position)
    {
        Debug.Log("show tool tip");
        inventoryUI.ShowTooltip(itemData, position);
    }

    public void HideTooltip()
    {
        Debug.Log("Hide tip");
        inventoryUI.HideTooltip();
    }
    #endregion

    #region Save and Load
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