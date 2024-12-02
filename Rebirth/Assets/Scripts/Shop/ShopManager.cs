using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShopManager : SingletonManager<ShopManager>
{
    [Header("Shop Items")]
    [SerializeField] private List<ItemData> allShopItems; // Master list of all items
    private List<ItemData> availableShopItems = new List<ItemData>(); // Current available shop items

    private ShopUI shopUI; 
    private bool shopUIActivated = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeShopUI();
        UpdateAvailableShopItems();
    }

    private void InitializeShopUI()
    {
        shopUI = FindObjectOfType<ShopUI>();
        if (shopUI == null)
        {
            Debug.LogError("ShopUI component not found in the scene.");
        }
        else
        {
            shopUI.gameObject.SetActive(false); // Start with shop UI hidden
        }
    }

    private void UpdateAvailableShopItems()
    {
        availableShopItems.Clear();
        int playerState = CharacterStatusManager.Instance.PlayerState;

        foreach (ItemData item in allShopItems)
        {
            // Add logic for filtering based on player state
            if (playerState == 1 && item.itemName == "2Dweirdpotion")
            {
                availableShopItems.Add(item);
            }
            else if (item.dimension == GetCurrentDimension())
            {
                availableShopItems.Add(item);
            }
        }
    }

    public void ToggleShopUI()
    {
        if (shopUI != null)
        {
            shopUIActivated = !shopUIActivated;
            if (shopUIActivated)
            {
                OpenShop();
            }
            else
            {
                CloseShop();
            }
        }
        else
        {
            Debug.LogError("ShopUI is not initialized.");
        }
    }

    private void OpenShop()
    {
        UpdateAvailableShopItems(); // Update the items before opening the shop
        shopUI.DisplayItems(availableShopItems);
        shopUI.gameObject.SetActive(true); // Show shop UI
        GameStateManager.Instance.LockView(); // Lock camera movement
    }

    private void CloseShop()
    {
        shopUI.gameObject.SetActive(false); // Hide shop UI
        GameStateManager.Instance.UnlockView(); // Unlock camera movement
    }

    public Dimension GetCurrentDimension()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.EndsWith("3D"))
        {
            return Dimension.THREE_DIMENSION;
        }
        else if (currentSceneName.EndsWith("2D"))
        {
            return Dimension.TWO_DIMENSION;
        }
        else
        {
            return Dimension.TWO_DIMENSION; // Default to 2D
        }
    }

    public bool PurchaseItem(ItemData itemData)
    {
        if (itemData == null) return false;

        if (CharacterStatusManager.Instance.Money >= itemData.price)
        {
            CharacterStatusManager.Instance.UpdateMoney(-itemData.price);
            InventoryManager.Instance.AddItem(itemData);

            // Remove the purchased item from the available shop items
            availableShopItems.Remove(itemData);

            return true; // Purchase successful
        }

        return false; // Purchase failed
    }
}
