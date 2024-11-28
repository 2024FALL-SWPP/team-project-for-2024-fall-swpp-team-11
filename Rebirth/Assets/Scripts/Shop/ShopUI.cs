using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform container; // Parent for item buttons
    [SerializeField] private Transform shopItemTemplate; // Template for buttons
    [SerializeField] private List<ItemData> shopItems; // List of item data
    [SerializeField] private GameObject shopPanel; // Reference to the shop UI panel
    private bool isShopOpen = false;

    private void Awake()
    {
        shopItemTemplate.gameObject.SetActive(false); // Hide template initially
    }

    private void Start()
    {
        // Automatically let Vertical Layout Group handle the positioning
        for (int i = 0; i < shopItems.Count; i++)
        {
            CreateItemButton(shopItems[i]);
        }
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
        shopPanel.SetActive(true); // Show shop UI
        GameStateManager.Instance.LockView(); // Lock camera movement
    }

    public void CloseShop()
    {
        isShopOpen = false;
        shopPanel.SetActive(false); // Hide shop UI
        GameStateManager.Instance.UnlockView(); // Unlock camera movement
    }

    private void CreateItemButton(ItemData itemData)
    {
        // Instantiate the template as a child of the container
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);

        // Assign item data to the UI elements
        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemData.itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemData.value.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemData.icon;
        shopItemTransform.Find("descriptionText").GetComponent<TextMeshProUGUI>().SetText(itemData.description);

        // Activate the item to make it visible
        shopItemTransform.gameObject.SetActive(true);
    }
}
