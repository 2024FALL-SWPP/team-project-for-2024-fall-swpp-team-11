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
    private InventoryData inventoryData;

    public void Initialize(InventoryData data)
    {
        inventoryData = data;
        itemTooltip = new ItemTooltip(tooltip);
    }

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

    public void RefreshInventoryDisplay()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventoryData.Items)
        {
            GameObject inventoryItem = Instantiate(inventoryItemPrefab, contentPanel);
            inventoryItem.GetComponent<InventoryItem>().Initialize(item);
        }
    }

    public void ShowTooltip(ItemData itemData, Vector2 position)
    {
        itemTooltip.Show();
        itemTooltip.Initialize(itemData);
    }

    public void HideTooltip()
    {
        itemTooltip.Hide();
    }
}