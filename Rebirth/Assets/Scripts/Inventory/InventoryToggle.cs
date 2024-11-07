using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject InventoryUI;
    private bool isInventoryVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryVisible = !isInventoryVisible;
            InventoryUI.SetActive(isInventoryVisible);
        }
    }
}
