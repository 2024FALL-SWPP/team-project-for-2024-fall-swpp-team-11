using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public PlayerData playerData = new PlayerData();
    public InventoryData inventoryData = new InventoryData();
    public List<QuestData> questDataList = new List<QuestData>();
}
