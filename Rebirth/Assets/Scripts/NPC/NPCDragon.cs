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
            CharactersStatusManager.Instance.EndingID = 1;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending2"){ 
            CharactersStatusManager.Instance.EndingID = 2;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending3"){ 
            CharactersStatusManager.Instance.EndingID = 3;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending4"){ 
            CharactersStatusManager.Instance.EndingID = 4;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending5"){ 
            CharactersStatusManager.Instance.EndingID = 5;
        }

        portal.Interact();
    }
}
