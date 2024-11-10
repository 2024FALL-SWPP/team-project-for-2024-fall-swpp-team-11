using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject InventoryCanvas;
    private bool isInventoryVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isInventoryVisible)
                ShowInventory();
            else
                sdfInventory();
            isInventoryVisible = !isInventoryVisible;
        }
    }

    private void ShowInventory()
    {
        InventoryCanvas.SetActive(true);
        GameStateManager.Instance.LockView();
    }

    private void sdfInventory()
    {
        InventoryCanvas.SetActive(false);
        GameStateManager.Instance.UnlockView(); 
    }
}
