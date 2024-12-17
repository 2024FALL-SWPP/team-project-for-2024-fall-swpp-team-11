using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Linq;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using UnityEditor;

public class DiskSaveSystem
{
    private static string logPrefix = "[DiskSaveSystem] ";

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
    
        Debug.Log(logPrefix + "Saving inventory data to disk.");
        Debug.Log(logPrefix + "Inventory data: " + json);
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

        Debug.Log(logPrefix + "Loaded inventory data from disk.");
        Debug.Log(logPrefix + "Inventory data: " + json);

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

        Debug.Log(logPrefix + $"Saving scene data to disk: {path}");
        Debug.Log(logPrefix + $"Scene data: {json}");
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

        Debug.Log(logPrefix + "Loaded all scene data from disk.");
        for (int i = 0; i < sceneDatas.Count; i++)
        {
            Debug.Log(logPrefix + $"Scene data {i}: {JsonUtility.ToJson(sceneDatas.ElementAt(i).Value)}");
        }

        return sceneDatas;
    }
    #endregion

    #region Character Status Management
    private static string CharacterStatusSavePath => Path.Combine(Application.persistentDataPath, "character_status.json");

    public static void SaveCharacterStatusToDisk()
    {
        string lastScene;
        lastScene = SceneManager.GetActiveScene().name;

        if (lastScene == "MainMenu" || lastScene == "EndingScene" || lastScene == "Narration")
            lastScene = "HeroHouse2D";

        CharacterStatusData characterStatusData = new CharacterStatusData
        {
            Money = CharacterStatusManager.Instance.Money,
            Health = CharacterStatusManager.Instance.Health,
            PlayerState = CharacterStatusManager.Instance.PlayerState,
            CanAccessLibrary = CharacterStatusManager.Instance.CanAccessLibrary,
            IsPaperSfixed = CharacterStatusManager.Instance.IsPaperSfixed,
            IsPaperEfixed = CharacterStatusManager.Instance.IsPaperEfixed,
            IsPaperBfixed = CharacterStatusManager.Instance.IsPaperBfixed,
            EndingID = CharacterStatusManager.Instance.EndingID,
            LastScene = lastScene
        };

        string json = JsonConvert.SerializeObject(characterStatusData, Formatting.Indented);
        File.WriteAllText(CharacterStatusSavePath, json);

        Debug.Log(logPrefix + "Saving character status to disk.");
        Debug.Log(logPrefix + "Character status: " + json);
    }

    private static string GetLastScene()
    {
        string lastScene;
        CharacterStatusData data = LoadCharacterStatusFromDisk();
        if (data.LastScene == "MainMenu" || data.LastScene == "Dungeon2D" || data.LastScene == "Dungeon3D")
            lastScene = "Village2D";
        else
            lastScene = data.LastScene;
        return lastScene;
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

        Debug.Log(logPrefix + "Loaded character status from disk.");
        Debug.Log(logPrefix + "Character status: " + json);
        return characterStatusData;
    }
    #endregion

    public static void ResetAllFiles()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath);

        foreach (string file in files)
        {
            // Debug.Log($"Deleting file: {file}");
            File.Delete(file);
        }

        Debug.Log(logPrefix + "All save files deleted.");
    }

    public static void ResetFilesExceptPlayerState()
    {
        DeleteAllSaveFilesExceptCharacterStatus();
        ResetCharacterStatusExceptPlayerState();
        
        Debug.Log(logPrefix + "All save files except player state deleted.");
    }

    public static void ResetCharacterStatusExceptPlayerState()
    {
        if (!File.Exists(CharacterStatusSavePath)) return;

        string json = File.ReadAllText(CharacterStatusSavePath);
        CharacterStatusData existingData = JsonConvert.DeserializeObject<CharacterStatusData>(json);

        CharacterStatusData resetData = new CharacterStatusData
        {
            PlayerState = existingData.PlayerState,
            Money = 0,
            Health = 100,
        };

        string updatedJson = JsonConvert.SerializeObject(resetData, Formatting.Indented);
        File.WriteAllText(CharacterStatusSavePath, updatedJson);
    }

    public static void DeleteAllSaveFilesExceptCharacterStatus()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath);

        foreach (string file in files)
        {
            if (file != CharacterStatusSavePath)
            {
                File.Delete(file);
            }
        }
    }

    #region NPC Met Status
    private static string NpcMetStatusSavePath => Path.Combine(Application.persistentDataPath, "npc_met_status.json");

    public static void SaveNPCMetStatusToDisk(ref Dictionary<string, bool> npcMetStatus)
    {        
        string json = JsonConvert.SerializeObject(npcMetStatus, Formatting.Indented);
        File.WriteAllText(NpcMetStatusSavePath, json);

        Debug.Log(logPrefix + "Saving NPC met status to disk.");
        Debug.Log(logPrefix + "NPC met status: " + json);
    }

    public static Dictionary<string, bool> LoadNPCMetStatusFromDisk()
    {
        if (!File.Exists(NpcMetStatusSavePath))
        {
            Debug.LogWarning("No NPC met status save file found.");
            return new Dictionary<string, bool>();
        }

        string json = File.ReadAllText(NpcMetStatusSavePath);

        Debug.Log(logPrefix + "Loaded NPC met status from disk.");
        Debug.Log(logPrefix + "NPC met status: " + json);

        return JsonConvert.DeserializeObject<Dictionary<string, bool>>(json) ?? new Dictionary<string, bool>();
    }
    #endregion


    #region  Quest Manager

    [System.Serializable]
    public class QuestSaveData
    {
        public int questID;
        public string questTitle;
        public string questDescription;
        public string rewardItemName;
        public string abilityUnlocked;
        public string requiredItemName;
    }
 
    private static string QuestSavePath => Path.Combine(Application.persistentDataPath, "quests.json");
    private static string QuestStatusSavePath => Path.Combine(Application.persistentDataPath, "quest_statuses.json");

    public static void SaveQuestManagerToDisk(Dictionary<int, QuestData> quests, Dictionary<int, QuestStatus> questStatuses)
    {
        // Save quests
        var questSaveData = new Dictionary<int, QuestSaveData>();
        foreach (var quest in quests)
        {
            questSaveData[quest.Key] = new QuestSaveData
            {
                questID = quest.Key,
                questTitle = quest.Value.questTitle,
                questDescription = quest.Value.questDescription,
                rewardItemName = quest.Value.rewardItem?.itemName,
                abilityUnlocked = quest.Value.abilityUnlocked,
                requiredItemName = quest.Value.requiredItem?.itemName
            };
        }
        string questJson = JsonConvert.SerializeObject(questSaveData, Formatting.Indented);
        File.WriteAllText(QuestSavePath, questJson);

        // Save quest statuses
        string statusJson = JsonConvert.SerializeObject(questStatuses, Formatting.Indented);
        File.WriteAllText(QuestStatusSavePath, statusJson);

        Debug.Log(logPrefix + "Saving quest manager to disk.");
        Debug.Log(logPrefix + "Quest data: " + questJson);
        Debug.Log(logPrefix + "Quest status data: " + statusJson);
    }

    public static async Task<(Dictionary<int, QuestData>, Dictionary<int, QuestStatus>)> LoadQuestManagerFromDiskAsync()
    {
        var quests = new Dictionary<int, QuestData>();
        var questStatuses = new Dictionary<int, QuestStatus>();

        if (File.Exists(QuestSavePath))
        {
            string questJson = File.ReadAllText(QuestSavePath);
            var questSaveData = JsonConvert.DeserializeObject<Dictionary<int, QuestSaveData>>(questJson) 
                ?? new Dictionary<int, QuestSaveData>();

            // Load all ItemData assets
            AsyncOperationHandle<IList<ItemData>> handle = Addressables.LoadAssetsAsync<ItemData>(
                "ItemData",
                null
            );

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load ItemData from Addressables.");
                Addressables.Release(handle);
                return (quests, questStatuses);
            }

            IList<ItemData> allItems = handle.Result;

            foreach (var kvp in questSaveData)
            {
                // Create a new ScriptableObject instance
                QuestData loadedQuest = ScriptableObject.CreateInstance<QuestData>();

                // Populate quest data
                loadedQuest.questID = kvp.Key;
                loadedQuest.questTitle = kvp.Value.questTitle;
                loadedQuest.questDescription = kvp.Value.questDescription;
                loadedQuest.abilityUnlocked = kvp.Value.abilityUnlocked;

                // Find and assign items by name
                loadedQuest.rewardItem = allItems.FirstOrDefault(i => i.itemName == kvp.Value.rewardItemName);
                loadedQuest.requiredItem = allItems.FirstOrDefault(i => i.itemName == kvp.Value.requiredItemName);

                quests[kvp.Key] = loadedQuest;
            }

            Addressables.Release(handle);
        }

        if (File.Exists(QuestStatusSavePath))
        {
            string statusJson = File.ReadAllText(QuestStatusSavePath);
            questStatuses = JsonConvert.DeserializeObject<Dictionary<int, QuestStatus>>(statusJson) 
                ?? new Dictionary<int, QuestStatus>();
        }

        Debug.Log(logPrefix + "Loaded quest manager from disk.");
        Debug.Log(logPrefix + "Quest data: " + JsonConvert.SerializeObject(quests, Formatting.Indented));
        Debug.Log(logPrefix + "Quest status data: " + JsonConvert.SerializeObject(questStatuses, Formatting.Indented));

        return (quests, questStatuses);
    }

    // 동기 버전의 로드 메서드 (필요한 경우)
    public static void LoadQuestManagerFromDisk(out Dictionary<int, QuestData> quests, out Dictionary<int, QuestStatus> questStatuses)
    {
        var loadTask = LoadQuestManagerFromDiskAsync();
        loadTask.Wait();

        (quests, questStatuses) = loadTask.Result;
    }

    #endregion
}
