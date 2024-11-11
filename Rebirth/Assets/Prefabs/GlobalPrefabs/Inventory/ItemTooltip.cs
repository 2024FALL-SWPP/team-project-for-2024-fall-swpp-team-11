using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip
{
    private GameObject tooltipObject;
    private Image itemIcon;
    private TMP_Text itemName;
    private TMP_Text itemDescription;

    public ItemTooltip(GameObject tooltipObject)
    {
        this.tooltipObject = tooltipObject;
        itemIcon = tooltipObject.transform.Find("ItemIcon").GetComponent<Image>();
        itemName = tooltipObject.transform.Find("ItemName").GetComponent<TMP_Text>();
        itemDescription = tooltipObject.transform.Find("ItemDescription").GetComponent<TMP_Text>();
    }

    public void Initialize(ItemData itemData)
    {
        itemIcon.sprite = itemData.icon;
        itemName.text = itemData.itemName;
        itemDescription.text = itemData.description;
    }

    public void Show() => tooltipObject.SetActive(true);
    public void Hide() => tooltipObject.SetActive(false);
}