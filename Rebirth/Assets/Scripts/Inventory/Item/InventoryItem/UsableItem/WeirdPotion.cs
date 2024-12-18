using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "WeirdPotion", menuName = "Item/Weird Potion")]
public class WeirdPotion : UsableItem
{
    public string targetSceneName = "HeroHouse2D"; // 삭제할 오브젝트가 있는 씬 이름
    public string targetObjectName = "2DDungeonKey"; // 삭제할 오브젝트의 이름

    public NPC sceneTransitionUsageNPC;

    public override void Use()
    {
        // 캐릭터 상태 변경
        CharacterStatusManager.Instance.SetPlayerState(PlayerState.IsToxified); // World Item will be invisible when state is IsToxified
        InventoryManager.Instance.RemoveItemByName("2DDungeonKey");

        // Scene Transition을 섦명
        NPC newNPC = Instantiate(sceneTransitionUsageNPC) as NPC;
        newNPC.Interact();
    }
}
