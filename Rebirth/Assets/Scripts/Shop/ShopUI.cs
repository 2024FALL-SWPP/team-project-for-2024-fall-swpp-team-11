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

    private void Awake()
    {
        if (shopItemTemplate != null)
        {
            shopItemTemplate.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Shop Item Template is not assigned in the inspector.");
        }

        if (shopPanel == null)
        {
            Debug.LogError("Shop Panel is not assigned in the inspector.");
        }
    }

    public void DisplayItems(List<ItemData> items)
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("No items to display in the shop.");
            return;
        }

        ClearItems();

        foreach (ItemData item in items)
        {
            CreateItemButton(item);
        }
    }

    private void ClearItems()
    {
        foreach (Transform child in container)
        {
            if (child != shopItemTemplate)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void CreateItemButton(ItemData itemData)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);

        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemData.itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemData.price.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemData.icon;
        shopItemTransform.Find("descriptionText").GetComponent<TextMeshProUGUI>().SetText(itemData.description);

        Button button = shopItemTransform.Find("Button").GetComponent<Button>();

        // Add listener to purchase the item when the button is clicked
        button.onClick.AddListener(() =>
        {
            ShopManager.Instance.PurchaseItem(itemData);
        });

        shopItemTransform.gameObject.SetActive(true);
    }
}
