using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDragon : NPC
{
    public Portal portal;
    public bool is2DKilled = false;
    public bool is2DAlive = false;

    public void Update() {
        if(!is2DAlive && !is2DKilled)
        {
            return;
        }

        gameObject.SetActive(false); 
        
        if(is2DAlive && CharacterStatusManager.Instance.PlayerState != 5){
            gameObject.SetActive(true);
        }
        if(is2DKilled && CharacterStatusManager.Instance.PlayerState == 5){
            gameObject.SetActive(true);
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