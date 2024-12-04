using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMagicList : NPC
{
    public Portal libraryPortal;
    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        if(DialogueManager.Instance.getLastLeafNode().nodeID == "library11"){ 
            libraryPortal.Interact();
            CharacterStatusManager.Instance.SetCanAccessLibrary(true);
        }

    }
}
