using UnityEngine;

[CreateAssetMenu(fileName = "Met NPC Condition", menuName = "Dialogue/Conditions/Met NPC")]
public class MetNPCBeforeCondition : DialogueCondition
{
    public string npcName;

    public override bool IsMet()
    {
        return NPCManager.Instance.HasMetNPC(npcName);
    }
}
