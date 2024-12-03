using UnityEngine;

public class StoreItemVisibilityController : MonoBehaviour
{
    public PlayerState playerStateWantToShow;
    public ItemData referenceItemData;

    void Update()
    {
        if (CharacterStatusManager.Instance.PlayerState == playerStateWantToShow && !ShopManager.Instance.IsItemPurchased(referenceItemData.itemName))
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    private void SetVisibility(bool visible)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}