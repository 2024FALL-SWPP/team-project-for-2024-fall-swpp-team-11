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
            EndingID = CharacterStatusManager.Instance.EndingID,
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

    #region NPC Met Status
    private static string NpcMetStatusSavePath => Path.Combine(Application.persistentDataPath, "npc_met_status.json");

    public static void SaveNPCMetStatusToDisk(ref Dictionary<string, bool> npcMetStatus)
    {        
        string json = JsonConvert.SerializeObject(npcMetStatus, Formatting.Indented);
        File.WriteAllText(NpcMetStatusSavePath, json);
    }

    public static void LoadNPCMetStatusFromDisk(ref Dictionary<string, bool> npcMetStatus)
    {
        if (!File.Exists(NpcMetStatusSavePath))
        {
            npcMetStatus = new Dictionary<string, bool>();
        }

        string json = File.ReadAllText(NpcMetStatusSavePath);
        npcMetStatus = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json) ?? new Dictionary<string, bool>();
    }
    #endregion


    #region  Quest Manager

    [System.Serializable]
    public class QuestSaveData
    {
        public int questID;
        public QuestData questData;
    }
 
    private static string QuestSavePath => Path.Combine(Application.persistentDataPath, "quests.json");
    private static string QuestStatusSavePath => Path.Combine(Application.persistentDataPath, "quest_statuses.json");

    public static void SaveQuestManagerToDisk(Dictionary<int, QuestData> quests, Dictionary<int, QuestStatus> questStatuses)
    {
        // Save quests
        var questList = new List<QuestSaveData>();
        foreach (var quest in quests)
        {
            questList.Add(new QuestSaveData
            {
                questID = quest.Key,
                questData = quest.Value
            });
        }
        string questJson = JsonConvert.SerializeObject(questList, Formatting.Indented);
        File.WriteAllText(QuestSavePath, questJson);

        // Save quest statuses
        string statusJson = JsonConvert.SerializeObject(questStatuses, Formatting.Indented);
        File.WriteAllText(QuestStatusSavePath, statusJson);
    }

    public static void LoadQuestManagerFromDisk(out Dictionary<int, QuestData> quests, out Dictionary<int, QuestStatus> questStatuses)
    {
        quests = new Dictionary<int, QuestData>();
        questStatuses = new Dictionary<int, QuestStatus>();

        if (File.Exists(QuestSavePath))
        {
            string questJson = File.ReadAllText(QuestSavePath);
            var questList = JsonConvert.DeserializeObject<List<QuestSaveData>>(questJson) ?? new List<QuestSaveData>();
            foreach (var questData in questList)
            {
                quests.Add(questData.questID, questData.questData);
            }
        }

        if (File.Exists(QuestStatusSavePath))
        {
            string statusJson = File.ReadAllText(QuestStatusSavePath);
            questStatuses = JsonConvert.DeserializeObject<Dictionary<int, QuestStatus>>(statusJson) ?? new Dictionary<int, QuestStatus>();
        }
    }

    #endregion
}
