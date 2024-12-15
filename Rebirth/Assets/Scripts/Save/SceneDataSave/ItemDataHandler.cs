using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemDataHandler : ISceneDataHandler
{
    private Dictionary<string, GameObject> prefabCache;

    public ItemDataHandler()
    {
        prefabCache = new Dictionary<string, GameObject>();
    }

    public Task CaptureDataAsync(SceneData sceneData)
    {
        foreach (var item in Object.FindObjectsOfType<WorldItem>(true))
        {
            Debug.Log($"Capturing data for item: {item.itemData.itemName}");
            sceneData.itemSceneDatas.Add(new ItemSceneData
            {
                itemName = item.itemData.itemName,
                position = item.transform.position
            });
        }
        return Task.CompletedTask;
    }

    public async Task ApplyDataAsync(SceneData sceneData)
    {
        var tasks = new List<Task>();

        foreach (var itemSceneData in sceneData.itemSceneDatas)
        {
            tasks.Add(LoadAndInstantiateAsync(itemSceneData));
        }

        await Task.WhenAll(tasks);
    }

    private async Task LoadAndInstantiateAsync(ItemSceneData itemSceneData)
    {
        if (!prefabCache.TryGetValue(itemSceneData.itemName, out var prefab))
        {
            prefab = await LoadPrefabAsync(itemSceneData.itemName);
            if (prefab == null)
            {
                Debug.LogError($"Failed to load prefab for item: {itemSceneData.itemName}");
                return;
            }

            prefabCache[itemSceneData.itemName] = prefab;
        }

        Object.Instantiate(prefab, itemSceneData.position, Quaternion.identity).name = itemSceneData.itemName;
    }

    private async Task<GameObject> LoadPrefabAsync(string itemName)
    {
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(
            "WorldItem", // Label 기반 검색
            null
        );

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var prefab in handle.Result)
            {
                Debug.Log($"Loaded prefab: {prefab.name}, target: {itemName}");
                if (prefab.name == itemName) // 이름 비교
                {                
                    return prefab;
                }
            }
        }

        Debug.LogError($"Prefab with name {itemName} not found in Addressables Group with label 'WorldItem'.");
        return null;
    }
}