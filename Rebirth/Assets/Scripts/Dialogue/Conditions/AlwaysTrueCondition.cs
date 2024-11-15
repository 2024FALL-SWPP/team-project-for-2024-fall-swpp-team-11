using UnityEngine;

[CreateAssetMenu(fileName = "AlwaysTrueCondition", menuName = "Dialogue/Conditions/Always True")]
public class AlwaysTrueCondition : DialogueCondition
{
    public override bool IsMet()
    {
        return true;
    }
}
