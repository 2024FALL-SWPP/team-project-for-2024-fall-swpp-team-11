using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [Header("Shop UI References")]
    [SerializeField] private Transform container; // Parent for item buttons
    [SerializeField] private Transform shopItemTemplate; // Template for buttons
    [SerializeField] private GameObject shopPanel; // Shop panel for showing UI
    [SerializeField] private string shopName; // Name of the shop interactable

    private List<ItemData> filteredItems = new List<ItemData>();
    private bool isShopOpen = false;

    private void Awake()
    {
        shopItemTemplate.gameObject.SetActive(false); 
    }

    public void SetShopName(string name)
    {
        shopName = name;
    }

    public void ToggleShopUI()
    {
        if (isShopOpen)
            CloseShop();
        else
            OpenShop();
    }

    public void OpenShop()
    {
        isShopOpen = true;
        RefreshShopItems(ShopManager.Instance.GetCurrentDimension(), shopName); // Pass shop name
        shopPanel.SetActive(true); // Show shop UI
        GameStateManager.Instance.LockView(); // Lock camera movement
    }

    public void CloseShop()
    {
        isShopOpen = false;
        shopPanel.SetActive(false); // Hide shop UI
        GameStateManager.Instance.UnlockView(); // Unlock camera movement
    }

    private void RefreshShopItems(Dimension currentDimension, string shopName)
    {
        filteredItems = ShopManager.Instance.GetItemsForDimension(currentDimension, shopName);

        // Clear old items
        foreach (Transform child in container)
        {
            if (child != shopItemTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        // Create buttons for filtered items
        foreach (ItemData item in filteredItems)
        {
            CreateItemButton(item);
        }
    }

    private void CreateItemButton(ItemData itemData)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);

        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemData.itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemData.value.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemData.icon;
        shopItemTransform.Find("descriptionText").GetComponent<TextMeshProUGUI>().SetText(itemData.description);

        Button button = shopItemTransform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            ShopManager.Instance.PurchaseItem(itemData);
        });

        shopItemTransform.gameObject.SetActive(true);
    }
}
