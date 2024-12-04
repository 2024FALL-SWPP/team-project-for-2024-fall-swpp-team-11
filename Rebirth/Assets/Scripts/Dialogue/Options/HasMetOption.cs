using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class HasMetOption : DialogueOption
{

    public HasMetOption(string NPCName)
    {
        onSelectActions.AddListener(() => {
            NPCManager.Instance.MarkNPCAsMet(NPCName);
        });
    }

}