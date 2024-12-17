using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "WeirdPotion", menuName = "Item/Weird Potion")]
public class WeirdPotion : UsableItem
{
    public string targetSceneName = "HeroHouse2D"; // 삭제할 오브젝트가 있는 씬 이름
    public string targetObjectName = "2DDungeonKey"; // 삭제할 오브젝트의 이름

    public NPC SceneTransitionUsageNPC; // 사용 이후 띄울 dialogue를 가진 NPC
    private bool hasInteractedWithSceneTransitionUsage = false; // 최초로 한 번만 실행.

    public override void Use()
    {
        // 캐릭터 상태 변경
        CharacterStatusManager.Instance.SetPlayerState(PlayerState.IsToxified); // World Item will be invisible when state is IsToxified
        InventoryManager.Instance.RemoveItemByName("2DDungeonKey");

        if (!hasInteractedWithSceneTransitionUsage) 
            {
                hasInteractedWithSceneTransitionUsage = true;

                NPC newNPC = Instantiate(SceneTransitionUsageNPC) as NPC;
                if (newNPC != null)
                {
                    newNPC.Interact();
                    return;
                }
                else
                {
                    Debug.LogError("NPC 프리팹이 제대로 할당되지 않았습니다.");
                    return;
                }
            } 
    }
}
