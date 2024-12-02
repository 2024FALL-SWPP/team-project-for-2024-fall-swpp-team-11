using UnityEngine;

[CreateAssetMenu(fileName = "Paper Fixed Condition", menuName = "Dialogue/Conditions/Paper Fixed")]
public class IsPaperFixedCondition : DialogueCondition
{
    public string sortOfPaper;
    public override bool IsMet()
    {        
        return CharacterStatusManager.Instance.GetPaper(sortOfPaper);
    }
}
