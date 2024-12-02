using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDungeonKey : NPC
{
    public Portal portal;
    public ItemData itemData;

    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        if(InventoryManager.Instance.HasItem(itemData) && DialogueManager.Instance.getLastLeafNode().nodeID == "dungeon")
        { 
            portal.Interact();
        }

    }
}
