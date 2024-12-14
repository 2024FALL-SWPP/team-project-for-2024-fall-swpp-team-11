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

        if(is2DAlive && CharacterStatusManager.Instance.PlayerState != PlayerState.IsDetoxified){
            gameObject.SetActive(true);
        }
        if(is2DKilled && CharacterStatusManager.Instance.PlayerState == PlayerState.IsDetoxified){
            gameObject.SetActive(true);
        }
    }

    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        
        if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending1"){ 
            CharacterStatusManager.Instance.EndingID = 1;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending2"){ 
            CharacterStatusManager.Instance.EndingID = 2;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending3"){ 
            CharacterStatusManager.Instance.EndingID = 3;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending4"){ 
            CharacterStatusManager.Instance.EndingID = 4;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending5"){ 
            CharacterStatusManager.Instance.EndingID = 5;
        } else if(DialogueManager.Instance.getLastLeafNode().nodeID == "ending6"){ 
            CharacterStatusManager.Instance.EndingID = 6;
        }
        Debug.Log(CharacterStatusManager.Instance.EndingID);

        portal.Interact();
    }
}
