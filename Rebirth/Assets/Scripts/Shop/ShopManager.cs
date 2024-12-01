using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Items")]
    [SerializeField] private List<ItemData> allShopItems; // Master list of all items

    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Enforce singleton
        }
    }

    /// <summary>
    /// Returns a filtered list of items based on the current dimension.
    /// </summary>
    /// <param name="dimension">The current dimension (2D or 3D).</param>
    /// <returns>List of filtered items for the dimension.</returns>
    public List<ItemData> GetItemsForDimension(Dimension dimension)
    {
        return allShopItems.FindAll(item => item.dimension == dimension);
    }

    /// <summary>
    /// Handles the logic for purchasing an item.
    /// </summary>
    /// <param name="itemData">The item to purchase.</param>
    /// <returns>True if the purchase was successful, otherwise false.</returns>
    public bool PurchaseItem(ItemData itemData)
    {
        CharacterStatusManager.Instance.UpdateMoney(-itemData.value);
        InventoryManager.Instance.AddItem(itemData);
        return true; // Purchase successful
    }

    /// <summary>
    /// Determines the current dimension based on the active scene name.
    /// </summary>
    /// <returns>The current dimension (2D or 3D).</returns>
    public Dimension GetCurrentDimension()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName.EndsWith("3D"))
        {
            return Dimension.THREE_DIMENSION;
        }
        else
        {
            return Dimension.TWO_DIMENSION; // Default to 2D
        }
    }
}
