using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMagicList : NPC
{
    public Portal libraryPortal;
    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        if(true){ // 후에 명부데이터 처리가 가능해지면 수정
            libraryPortal.Interact();
        }

    }
}
