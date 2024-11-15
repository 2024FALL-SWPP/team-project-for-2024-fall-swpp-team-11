using UnityEngine;

[CreateAssetMenu(fileName = "Has Item Condition", menuName = "Dialogue/Conditions/Has Item")]
public class HasItemCondition : DialogueCondition
{
    public ItemData requiredItem;

    public override bool IsMet()
    {
        return InventoryManager.Instance.HasItem(requiredItem);
    }
}
