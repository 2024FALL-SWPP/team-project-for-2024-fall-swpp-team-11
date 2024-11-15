using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/savefile.json";
    private static string InventorySavePath => Application.persistentDataPath + "/inventory.json";

    public static void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("저장됨!");
    
    }

    public static GameData LoadGame()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            Debug.Log("저장 파일을 찾을 수 없습니다!");
            return null;
        }
    }

   
    public static void SaveInventoryData(InventoryData inventoryData)
    {
        List<string> itemNames = new List<string>();
        foreach (var item in inventoryData.Items)
        {
            itemNames.Add(item.itemName);
        }

        string json = JsonConvert.SerializeObject(itemNames, Formatting.Indented);
        Debug.Log("Items in inventory:" + json);

        File.WriteAllText(InventorySavePath, json);
        Debug.Log("인벤토리 아이템들이 파일로 저장되었습니다.");
    }
    
    public static InventoryData LoadInventoryData()
    {
        if (!File.Exists(InventorySavePath))
        {
            Debug.LogWarning("Inventory JSON 파일이 존재하지 않습니다.");
            return null;
        }

        string json = File.ReadAllText(InventorySavePath);
        List<string> itemNames = JsonConvert.DeserializeObject<List<string>>(json);

        // Resources 폴더에서 모든 ItemData 로드return null;
        ItemData[] allItems = Resources.LoadAll<ItemData>("ScriptableItem");

        // 이름 목록과 일치하는 ItemData 필터링
        // 이름 목록과 일치하는 ItemData 필터링
        List<ItemData> loadedItems = new List<ItemData>();
        foreach (var itemName in itemNames)
        {
            // 이름이 일치하는 아이템을 찾아서 추가
            ItemData foundItem = System.Array.Find(allItems, item => item.itemName == itemName);
            if (foundItem != null)
            {
                loadedItems.Add(foundItem);
            }
            else
            {
                Debug.LogWarning($"아이템 '{itemName}'을(를) 찾을 수 없습니다.");
            }
        }
        return null;
    }

}