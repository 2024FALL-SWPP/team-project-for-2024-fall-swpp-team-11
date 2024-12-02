using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Linq;

public class DiskSaveSystem
{
    #region Inventory Management
    public static void SaveInventoryDataToDisk(InventoryDataContainer inventoryData)
    {
        string savePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        List<string> itemNames = new List<string>();
        foreach (var item in inventoryData.GetItems(Dimension.TWO_DIMENSION))
            itemNames.Add(item.itemName);
        foreach (var item in inventoryData.GetItems(Dimension.THREE_DIMENSION))
            itemNames.Add(item.itemName);

        string json = JsonConvert.SerializeObject(itemNames, Formatting.Indented);

        File.WriteAllText(savePath, json);
        Debug.Log($"Inventory saved to {savePath}");
    }

    public static async Task<List<ItemData>> LoadInventoryDataFromDiskAsync()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No inventory save file found. Returning empty list.");
            return new List<ItemData>();
        }

        string json = File.ReadAllText(savePath);

        List<string> itemNames = JsonConvert.DeserializeObject<List<string>>(json);

        AsyncOperationHandle<IList<ItemData>> handle = Addressables.LoadAssetsAsync<ItemData>(
            "ItemData",
            null
        );

        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to load ItemData from Addressables.");
            return new List<ItemData>();
        }

        IList<ItemData> allItems = handle.Result;
        List<ItemData> loadedItems = new List<ItemData>();

        foreach (string itemName in itemNames)
        {
            ItemData item = allItems.FirstOrDefault(i => i.itemName == itemName);
            if (item != null)
                loadedItems.Add(item);
            else
                Debug.LogWarning($"Item with name '{itemName}' not found in Addressables.");
        }

        Addressables.Release(handle);

        Debug.Log("Inventory loaded successfully.");
        return loadedItems;
    }
    #endregion

    #region Scene Management
    public static string GetSceneDataPath(string sceneName) =>
        Path.Combine(Application.persistentDataPath, $"{sceneName}_sceneData.json");

    public static void SaveSceneDataToDisk(string path, SceneData sceneData)
    {
        var json = JsonUtility.ToJson(sceneData);
        File.WriteAllText(path, json);
        Debug.Log($"Scene data saved to {path}");
    }

    public static async Task<Dictionary<string, SceneData>> LoadAllSceneDataFromDisk()
    {
        var sceneDatas = new Dictionary<string, SceneData>();
        var files = Directory.GetFiles(Application.persistentDataPath, "*_sceneData.json");

        var tasks = new List<Task>();
        foreach (var file in files)
        {
            tasks.Add(Task.Run(() =>
            {
                var sceneName = Path.GetFileNameWithoutExtension(file).Replace("_sceneData", "");
                var json = File.ReadAllText(file);
                var sceneData = JsonUtility.FromJson<SceneData>(json);
                lock (sceneDatas)
                {
                    sceneDatas[sceneName] = sceneData;
                }
            }));
        }
        await Task.WhenAll(tasks);

        Debug.Log("All scene data loaded.");
        return sceneDatas;
    }
    #endregion

    #region Character Status Management
    private static string CharacterStatusSavePath => Path.Combine(Application.persistentDataPath, "character_status.json");

    public static void SaveCharacterStatusToDisk()
    {
        CharacterStatusData characterStatusData = new CharacterStatusData
        {
            Money = CharacterStatusManager.Instance.Money,
            Health = CharacterStatusManager.Instance.Health,
            // IsDimensionSwitchable = CharactersStatusManager.Instance.IsDimensionSwitchable
            PlayerState = CharacterStatusManager.Instance.PlayerState,
            CanAccessLibrary = CharacterStatusManager.Instance.CanAccessLibrary,
            IsPaperSfixed = CharacterStatusManager.Instance.IsPaperSfixed,
            IsPaperEfixed = CharacterStatusManager.Instance.IsPaperEfixed,
            IsPaperBfixed = CharacterStatusManager.Instance.IsPaperBfixed,
            EndingID = CharactersStatusManager.Instance.EndingID,
        };

        string json = JsonConvert.SerializeObject(characterStatusData, Formatting.Indented);
        File.WriteAllText(CharacterStatusSavePath, json);
        Debug.Log("Character status saved to disk.");
    }

    public static CharacterStatusData LoadCharacterStatusFromDisk()
    {
        if (!File.Exists(CharacterStatusSavePath))
        {
            Debug.LogWarning("Character status file not found. Returning default values.");
            return new CharacterStatusData();
        }

        string json = File.ReadAllText(CharacterStatusSavePath);
        CharacterStatusData characterStatusData = JsonConvert.DeserializeObject<CharacterStatusData>(json);
        Debug.Log("Character status loaded from disk.");
        return characterStatusData;
    }
    #endregion
}
