using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject InventoryCanvas;
    private bool isInventoryVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryVisible = !isInventoryVisible;
            InventoryCanvas.SetActive(isInventoryVisible);
        }
    }
}
