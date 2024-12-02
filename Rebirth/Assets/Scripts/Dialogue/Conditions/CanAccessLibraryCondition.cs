using UnityEngine;

[CreateAssetMenu(fileName = "Library entry Condition", menuName = "Dialogue/Conditions/Library Entry")]
public class CanAccessLibraryCondition : DialogueCondition
{
    // public int levelToMeet;
    public override bool IsMet()
    {        
        return CharacterStatusManager.Instance.GetCanAccessLibrary();
    }
}
