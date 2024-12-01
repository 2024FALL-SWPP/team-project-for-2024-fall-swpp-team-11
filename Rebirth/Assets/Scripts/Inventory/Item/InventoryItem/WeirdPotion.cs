using UnityEngine;

[CreateAssetMenu(fileName = "WeirdPotion", menuName = "Item/Weird Potion")]
public class WeirdPotion : UsableItem
{
    public override void Use()
    {
        Debug.Log($"Use Weird Potion");
    }
}
