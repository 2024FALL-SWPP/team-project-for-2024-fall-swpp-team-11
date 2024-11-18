using UnityEngine;

[CreateAssetMenu(fileName = "Met NPC Condition", menuName = "Dialogue/Conditions/Met NPC")]
public class PlayerLevelCondition : DialogueCondition
{
    public int levelToMeet;

    public override bool IsMet()
    {
        // return PlayerData.Instance.level >= levelToMeet; TODO
        return false;
    }
}
