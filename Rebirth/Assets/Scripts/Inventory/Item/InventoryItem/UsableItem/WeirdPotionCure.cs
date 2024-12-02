using UnityEngine;

[CreateAssetMenu(fileName = "WeirdPotionCure", menuName = "Item/Weird Potion Cure")]
public class WeirdPotionCure : UsableItem
{
    public override void Use()
    {
        CharacterStatusManager.Instance.SetPlayerState(PlayerState.IsDetoxified);
    }
}
