using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShopManager : SingletonManager<ShopManager>
{
    [Header("Shop Items")]
    [SerializeField] private List<ItemData> allShopItems; // Master list of all items

    public List<ItemData> GetItemsForDimension(Dimension dimension, string shopName = null)
    {
        // Filter items by dimension first
        List<ItemData> itemsForDimension = allShopItems.FindAll(item => item != null && item.dimension == dimension);

        if (!string.IsNullOrEmpty(shopName))
        {
            shopName = shopName.ToLower();
            if (shopName == "shelve")
            {
                // Only include Shelve-specific items
                itemsForDimension = itemsForDimension.FindAll(item =>
                    item.itemName == "Dragon Comb" || item.itemName == "Pikachu Feather");
            }
            else
            {
                // Exclude Shelve-specific items from general shops
                itemsForDimension = itemsForDimension.FindAll(item =>
                    item.itemName != "Dragon Comb" && item.itemName != "Pikachu Feather");
            }
        }

        return itemsForDimension;
    }

    public bool PurchaseItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        if (CharacterStatusManager.Instance.Money >= itemData.value)
        {
            // Deduct money
            CharacterStatusManager.Instance.UpdateMoney(-itemData.value);

            // Add item to inventory
            InventoryManager.Instance.AddItem(itemData);

            return true; // Purchase successful
        }
        else
        {
            return false; // Purchase failed
        }
    }

    public void AddItemToShop(ItemData itemData)
    {
        if (itemData != null && !allShopItems.Contains(itemData))
        {
            allShopItems.Add(itemData);
        }
    }

    public void RemoveItemFromShop(ItemData itemData)
    {
        if (itemData != null && allShopItems.Contains(itemData))
        {
            allShopItems.Remove(itemData);
        }
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

    public bool CanAffordItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        return CharacterStatusManager.Instance.Money >= itemData.value;
    }

    public bool IsItemInShop(ItemData itemData)
    {
        return allShopItems.Contains(itemData);
    }

    public List<ItemData> GetAllItems()
    {
        return new List<ItemData>(allShopItems);
    }
}
