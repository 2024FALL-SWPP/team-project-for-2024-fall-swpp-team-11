using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShopManager : SingletonManager<ShopManager>
{
    [Header("Shop Items")]
    [SerializeField] private List<ItemData> allShopItems; // Master list of all items
    private Dictionary<string, ActivateItem> itemActivators = new Dictionary<string, ActivateItem>(); // Map of items and their activators

    private ShopUI shopUI; 
    private bool shopUIActivated = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeShopUI();
        InitializeItemActivators();
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

    private void InitializeItemActivators()
    {
        foreach (var item in FindObjectsOfType<ActivateItem>())
        {
            if (!itemActivators.ContainsKey(item.name))
            {
                itemActivators.Add(item.name, item);
                item.Deactivate(); // Ensure all items are deactivated initially
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
                // Check conditions based on PlayerState
                if (currentState == PlayerState.CanUseWeirdPotion && item.itemName == "2DWeirdPotion")
                {
                    ActivateItem(item.itemName);
                    itemsForDimension.Add(item);
                }
                else if (currentState == PlayerState.CanUseWeirdPotionCure && item.itemName == "2DWeirdPotion-Cure")
                {
                    ActivateItem(item.itemName);
                    itemsForDimension.Add(item);
                }
                else if (item.itemName != "2DWeirdPotion" && item.itemName != "2DWeirdPotion-Cure") // General items
                {
                    itemsForDimension.Add(item);
                }
                else
                {
                    DeactivateItem(item.itemName); // Deactivate items not meeting the condition
                }
            }
        }

        return itemsForDimension;
    }

    public void ActivateItem(string itemName)
    {
        if (itemActivators.TryGetValue(itemName, out var activator))
        {
            activator.Activate();
        }
        else
        {
            Debug.LogWarning($"Activator for item {itemName} not found.");
        }
    }

    public void DeactivateItem(string itemName)
    {
        if (itemActivators.TryGetValue(itemName, out var activator))
        {
            activator.Deactivate();
        }
    }

    public bool PurchaseItem(ItemData itemData)
    {
        if (itemData == null) return false;

        if (CharacterStatusManager.Instance.Money >= itemData.price)
        {
            CharacterStatusManager.Instance.UpdateMoney(-itemData.price);
            InventoryManager.Instance.AddItem(itemData);

            // Handle specific items based on their name
            if (itemData.itemName == "2DWeirdPotion")
            {
                MarkItemAsUsed("2DWeirdPotion");
                DeactivateItem("2DWeirdPotion");
            }
            else if (itemData.itemName == "2DWeirdPotion-Cure")
            {
                MarkItemAsUsed("2DWeirdPotion-Cure");
                DeactivateItem("2DWeirdPotion-Cure");
            }

            return true; // Purchase successful
        }

        return false; // Purchase failed
    }

    private void MarkItemAsUsed(string itemName)
    {
        if (itemActivators.ContainsKey(itemName))
        {
            itemActivators[itemName].Deactivate();
        }
    }
}
