using UnityEngine;

[CreateAssetMenu(fileName = "WeirdPotionCure", menuName = "Item/Weird Potion Cure")]
public class WeirdPotionCure : UsableItem
{
    public override async void Use()
    {
        CharacterStatusManager.Instance.SetPlayerState(PlayerState.IsDetoxified);

        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            await DimensionManager.Instance.SwitchDimension();
        }
    }
}
