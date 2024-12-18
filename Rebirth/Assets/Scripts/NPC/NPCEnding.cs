using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEnding : NPC
{
    public AudioSource audioSource;
    public AudioClip eventMusic;
    public Portal portal;

    public override void Interact()
    {
        audioSource.Play();
        base.Interact();
    }
    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        portal.Interact();       
    }
}
