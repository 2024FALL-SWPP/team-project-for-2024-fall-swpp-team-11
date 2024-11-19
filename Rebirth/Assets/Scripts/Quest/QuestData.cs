using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Create New Quest")]
public class QuestData : ScriptableObject
{
    public int questID;
    public string questTitle;
    public string questDescription;
    public ItemData rewardItem;
    public string abilityUnlocked;
    public ItemData requiredItem;
}
