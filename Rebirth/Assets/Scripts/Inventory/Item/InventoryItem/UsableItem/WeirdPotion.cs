using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "WeirdPotion", menuName = "Item/Weird Potion")]
public class WeirdPotion : UsableItem
{
    public string targetSceneName = "HeroHouse2D"; // 삭제할 오브젝트가 있는 씬 이름
    public string targetObjectName = "2DDungeonKey"; // 삭제할 오브젝트의 이름

    public override void Use()
    {
        // 캐릭터 상태 변경
        CharacterStatusManager.Instance.SetPlayerState(PlayerState.IsToxified); // 3:isToxified

        // 씬에서 오브젝트 삭제
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            if (SceneManager.GetActiveScene().name == targetSceneName)
            {
                // 현재 씬에서 오브젝트 삭제
                DeleteObjectInCurrentScene();
            }
            else
            {
                // 다른 씬을 로드하여 오브젝트 삭제
                SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive).completed += (operation) =>
                {
                    Debug.Log($"{targetSceneName} 씬 로드 완료");

                    GameObject targetObject = FindGameObjectInScene(targetSceneName, targetObjectName);
                    if (targetObject != null)
                    {
                        Object.Destroy(targetObject);
                        Debug.Log($"{targetObjectName} 오브젝트가 삭제되었습니다.");
                    }
                    else
                    {
                        Debug.LogWarning($"{targetObjectName} 오브젝트를 {targetSceneName}에서 찾을 수 없습니다.");
                    }

                    SceneManager.UnloadSceneAsync(targetSceneName).completed += (unloadOperation) =>
                    {
                        Debug.Log($"{targetSceneName} 씬 언로드 완료");
                    };
                };
            }
        }
        else
        {
            Debug.LogWarning("삭제할 씬 이름이 설정되지 않았습니다.");
        }

        // 인벤토리에서 아이템 삭제
        InventoryManager.Instance.RemoveItemByName("2DDungeonKey");
    }

    private void DeleteObjectInCurrentScene()
    {
        GameObject targetObject = GameObject.Find(targetObjectName);
        if (targetObject != null)
        {
            Object.Destroy(targetObject);
            Debug.Log($"{targetObjectName} 오브젝트가 현재 씬에서 삭제되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{targetObjectName} 오브젝트를 현재 씬에서 찾을 수 없습니다.");
        }
    }

    private GameObject FindGameObjectInScene(string sceneName, string objectName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                if (obj.name == objectName)
                {
                    return obj;
                }
            }
        }
        return null;
    }
}
