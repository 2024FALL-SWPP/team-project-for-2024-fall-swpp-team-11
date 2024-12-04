using UnityEngine;

public class NPCIDCard : NPC
{
    public ItemData itemData;
    private bool hasDisappeared = false; // 사라진 상태를 저장

    void Update()
    {
        if (!hasDisappeared && InventoryManager.Instance.HasItem(itemData))
        {
            gameObject.SetActive(false);
            hasDisappeared = true; 
   
        }
    }
}
