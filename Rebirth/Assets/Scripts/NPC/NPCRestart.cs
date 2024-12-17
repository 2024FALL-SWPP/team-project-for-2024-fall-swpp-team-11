using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRestart : NPC
{
    public GameObject MainMenuController;

    public override void HandleDialogueEnd(){
        base.HandleDialogueEnd();
        if(DialogueManager.Instance.getLastLeafNode().nodeID == "Restart"){
            MainMenuController script = MainMenuController.GetComponent<MainMenuController>();
            if(script != null) {
                script.RestartGame();
            }
        }
        GameStateManager.Instance.LockView();
    }
}
