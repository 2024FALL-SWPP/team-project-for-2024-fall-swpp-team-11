using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDragon : NPC
{
    public Portal portal;
    public bool is2DKilled = false;
    public bool is2DAlive = false;

    public void Update() {
        if(is2DAlive && CharacterStatusManager.Instance.PlayerState == ){

        }
    }

    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        
        if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending1"){ 
     
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending2"){ 
     
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending3"){ 
     
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending4"){ 
     
        }

        portal.Interact();
    }
}
