using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData playerData = new PlayerData();
    public List<QuestData> questDataList = new List<QuestData>();
}
