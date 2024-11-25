using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGod : NPC
{
    public ItemData itemData;
    public GodMoving godMovesUp;

    public override void Interact()
    {
        base.Interact();
        //우물에 있는 산신령이 특정 조건을 만족하면 위로 올라옴.
        if(InventoryManager.Instance.HasItem(itemData)){
            godMovesUp.MovingUp();
        }
    }
    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();

        if(InventoryManager.Instance.HasItem(itemData)){
            godMovesUp.MovingDown();
        }
    }

}
