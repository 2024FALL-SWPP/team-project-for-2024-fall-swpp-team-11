using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDragon : NPC
{
    public Portal portal;
    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        
        if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending1"){ 
     
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending2"){ 
     
        }


        portal.Interact();
    }
}
