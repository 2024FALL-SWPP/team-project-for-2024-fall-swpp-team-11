using UnityEngine;

[CreateAssetMenu(fileName = "UsableItem", menuName = "Item/Usable Item")]
public class UsableItem : ItemData, IUsable
{
    public int effectValue;

    public virtual void Use()
    {
        Debug.Log($"Using {itemName} with effect value: {effectValue}");
    }
}