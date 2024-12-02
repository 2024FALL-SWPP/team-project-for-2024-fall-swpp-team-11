using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShopManager : SingletonManager<ShopManager>
{
    [Header("Shop Items")]
    [SerializeField] private List<ItemData> allShopItems; // Master list of all items]
    [SerializeField] private ActivateItem weirdPotion; // Reference to the 2DWeirdPotion ActivateItem
    [SerializeField] private ActivateItem weirdPotionCure; // Reference to the 2DWeirdPotionCure ActivateItem

    private ShopUI shopUI; 
    private bool shopUIActivated = false;

    // Track purchased items
    private HashSet<string> purchasedItems = new HashSet<string>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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

    public List<ItemData> GetItemsForDimension(Dimension dimension)
    {
        List<ItemData> itemsForDimension = new List<ItemData>();
        PlayerState currentState = CharacterStatusManager.Instance.PlayerState;

        foreach (var item in allShopItems)
        {
            if (item != null && item.dimension == dimension)
            {
                // Check conditions based on PlayerState and if the item is already purchased
                if ((currentState == PlayerState.CanUseWeirdPotion && item.itemName == "2DWeirdPotion" && !purchasedItems.Contains(item.itemName)) ||
                    (currentState == PlayerState.CanUseWeirdPotionCure && item.itemName == "2DWeirdPotionCure" && !purchasedItems.Contains(item.itemName)))
                {
                    itemsForDimension.Add(item); // Add item based on state and if not purchased
                }
                else if (item.itemName != "2DWeirdPotion" && item.itemName != "2DWeirdPotionCure")
                {
                    itemsForDimension.Add(item); // Add general items
                }
            }
        }

        return itemsForDimension;
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

    public void HandleItemStateChange(PlayerState newState)
    {
        if (weirdPotion != null)
        {
            if (newState == PlayerState.CanUseWeirdPotion)
            {
                weirdPotion.Activate(); // Activate Weird Potion
            }
            else
            {
                weirdPotion.Deactivate(); // Deactivate Weird Potion
            }
        }

        if (weirdPotionCure != null)
        {
            if (newState == PlayerState.CanUseWeirdPotionCure)
            {
                weirdPotionCure.Activate(); // Activate Weird Potion Cure
            }
            else
            {
                weirdPotionCure.Deactivate(); // Deactivate Weird Potion Cure
                Debug.Log($"Deactivated: {weirdPotionCure.gameObject.name}");
            }
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

        // Handle the activation/deactivation of items
        HandleItemStateChange(nextState);
    }
}
