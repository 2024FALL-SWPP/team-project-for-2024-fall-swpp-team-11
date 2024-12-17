using UnityEngine;

[CreateAssetMenu(fileName = "WeirdPotionCure", menuName = "Item/Weird Potion Cure")]
public class WeirdPotionCure : UsableItem
{
    public NPC SceneTransitionUsageExitNPC; // 사용 이후 띄울 dialogue를 가진 NPC

    public override async void Use()
    {
        CharacterStatusManager.Instance.SetPlayerState(PlayerState.IsDetoxified);

        if (DimensionManager.Instance.GetCurrentDimension() == Dimension.THREE_DIMENSION)
        {
            await DimensionManager.Instance.SwitchDimension();
        }

        NPC newNPC = Instantiate(SceneTransitionUsageExitNPC) as NPC;
        if (newNPC != null)
        {
            newNPC.Interact();
        }
        else
        {
            Debug.LogError("NPC 프리팹이 제대로 할당되지 않았습니다.");
            return;
        }
    }
}
