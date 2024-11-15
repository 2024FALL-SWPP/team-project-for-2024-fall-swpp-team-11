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

    public bool HasItem(ItemData item)
    {
        return inventoryData.HasItem(item);
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
}