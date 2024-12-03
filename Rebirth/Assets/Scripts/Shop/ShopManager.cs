using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShopManager : SingletonManager<ShopManager>
{
    [Header("Shop Items")]
    [SerializeField] private List<ItemData> allShopItems; // Master list of all items]

    private readonly string weirdPotion2DName = "2DWeirdPotion";
    private readonly string weirdPotionCure2DName = "2DWeirdPotionCure";
    private readonly string weirdPotionCure3DName = "3DWeirdPotionCure";

    private ShopUI shopUI; 
    private bool shopUIActivated = false;

    // Track purchased items
    private HashSet<string> purchasedItems = new HashSet<string>();

    protected override void Awake()
    {
        base.Awake();
        InitializeShopUI();
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
        Dimension currentDimension = GetCurrentDimension();
        List<ItemData> itemsToDisplay = GetItemsForDimension(currentDimension);

        shopUI.DisplayItems(itemsToDisplay);

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

    public bool IsWeirdPotionCurePurchased()
    {
        return purchasedItems.Contains(weirdPotionCure2DName) || purchasedItems.Contains(weirdPotionCure3DName);
    }

    public bool IsWeirdPotionPurchased()
    {
        return purchasedItems.Contains(weirdPotion2DName);
    }

    public bool IsItemPurchased(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return false;
        if (itemName == weirdPotionCure2DName || itemName == weirdPotionCure3DName)
        {
            return IsWeirdPotionCurePurchased();
        }
        else if (itemName == weirdPotion2DName)
        {
            return IsWeirdPotionPurchased();
        }
        return purchasedItems.Contains(itemName);
    }

    public List<ItemData> GetItemsForDimension(Dimension dimension)
    {
        PlayerState currentState = CharacterStatusManager.Instance.PlayerState;
        List<ItemData> itemsInDimension = allShopItems.FindAll(item => item != null && item.dimension == dimension);
        List<ItemData> itemsToDisplay = new List<ItemData>();

        foreach (var item in itemsInDimension)
        {
            if (item.itemName == weirdPotion2DName)
            {
                if (currentState == PlayerState.CanUseWeirdPotion && !IsWeirdPotionPurchased())
                {
                    itemsToDisplay.Add(item);
                }
            }
            else if (item.itemName == weirdPotionCure2DName || item.itemName == weirdPotionCure3DName)
            {
                // weirdPotionCure is available in both 2D and 3D dimensions, but only if the player has not purchased any of them
                if (currentState == PlayerState.CanUseWeirdPotionCure && !IsWeirdPotionCurePurchased())
                {
                    itemsToDisplay.Add(item);
                }
            }
            else
            {
                // Add general items
                itemsToDisplay.Add(item);
            }
        }

        return itemsToDisplay;
    }

    public bool PurchaseItem(ItemData itemData)
    {
        if (itemData == null) return false;

        // Ensure sufficient funds before attempting to purchase
        if (CharacterStatusManager.Instance.Money >= itemData.price)
        {
            // Check if the inventory has space
            if (InventoryManager.Instance.IsInventoryFull())
            {
                Debug.LogWarning("Cannot purchase more items. Inventory is full.");
                return false; // Prevent purchase if inventory is full
            }

            // Deduct money and add item to inventory
            CharacterStatusManager.Instance.UpdateMoney(-itemData.price);
            InventoryManager.Instance.AddItem(itemData);

            // Optionally track purchased items for reference (not restricting repeats)
            purchasedItems.Add(itemData.itemName);

            Debug.Log($"Item {itemData.itemName} purchased successfully.");
            return true; // Purchase successful
        }

        Debug.LogWarning("Not enough money to purchase this item.");
        return false; // Purchase failed due to insufficient funds
    }




    private void Update()
    {
        // Check for 'K' key press
        if (Input.GetKeyDown(KeyCode.K))
        {
            UpdatePlayerState();
        }
    }

    private void UpdatePlayerState()
    {
        PlayerState currentState = CharacterStatusManager.Instance.PlayerState;
        PlayerState[] states = (PlayerState[])System.Enum.GetValues(typeof(PlayerState));
        int nextIndex = (System.Array.IndexOf(states, currentState) + 1) % states.Length;

        PlayerState nextState = states[nextIndex];
        CharacterStatusManager.Instance.SetPlayerState(nextState);

        Debug.Log($"Player state updated to: {nextState}");
    }
}
