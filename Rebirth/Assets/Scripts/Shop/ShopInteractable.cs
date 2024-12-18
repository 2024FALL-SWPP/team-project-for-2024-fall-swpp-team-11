using UnityEngine;

public class ShopInteractable : MonoBehaviour, IInteractable
{
    // [SerializeField] private GameObject shopUI;
    private Outline outline;
    private Outline2D outline2D;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline2D = GetComponent<Outline2D>();
    }


    public void Interact()
    {
      
        ShopManager.Instance.ToggleShopUI();
    }

    public void OnFocus()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            if (!outline) return;
            outline.enabled = true;
        }
        else
        {
            if (!outline2D) return;
            outline2D.SetOutline();
        }
    }

    public void OnDefocus()
    {
        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            if (!outline) return;
            outline.enabled = false;
        }
        else
        {
            if (!outline2D) return;
            outline2D.UnsetOutline();
        }
    }
}
