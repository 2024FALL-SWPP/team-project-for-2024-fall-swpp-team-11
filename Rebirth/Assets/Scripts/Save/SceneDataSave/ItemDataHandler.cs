using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ItemDataHandler : ISceneDataHandler
{
    private Dictionary<string, GameObject> prefabCache;

    public ItemDataHandler()
    {
        InitializePrefabCache();
    }

    public Task CaptureData(SceneData sceneData)
    {
        foreach (var item in Object.FindObjectsOfType<WorldItem>(true))
        {
            sceneData.itemSceneDatas.Add(new ItemSceneData
            {
                itemName = item.itemData.itemName,
                position = item.transform.position
            });
        }
        return Task.CompletedTask;
    }

    public void ApplyData(SceneData sceneData)
    {
        foreach (var itemSceneData in sceneData.itemSceneDatas)
        {
            if (prefabCache.TryGetValue(itemSceneData.itemName, out var prefab))
            {
                Object.Instantiate(prefab, itemSceneData.position, Quaternion.identity).name = itemSceneData.itemName;
            }
        }
    }

    private void InitializePrefabCache()
    {
        prefabCache = new Dictionary<string, GameObject>();
        foreach (var prefab in Resources.LoadAll<GameObject>("WorldItemPrefabs"))
        {
            var worldItem = prefab.GetComponent<WorldItem>();
            if (worldItem?.itemData != null)
            {
                prefabCache[worldItem.itemData.itemName] = prefab;
            }
        }
    }
}
