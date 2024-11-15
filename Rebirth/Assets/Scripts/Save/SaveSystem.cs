using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/savefile.json";

    public static void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(SavePath, json);
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
}
