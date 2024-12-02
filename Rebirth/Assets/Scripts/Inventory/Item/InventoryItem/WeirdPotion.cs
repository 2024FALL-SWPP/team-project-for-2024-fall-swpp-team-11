using UnityEngine;

[CreateAssetMenu(fileName = "WeirdPotion", menuName = "Item/Weird Potion")]
public class WeirdPotion : UsableItem
{
    public override void Use()
    {
        CharacterStatusManager.Instance.SetIsDimensionSwitchable(true);
    }
}
