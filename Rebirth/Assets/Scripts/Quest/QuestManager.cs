using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus
{
    NotStarted,
    Accepted,
    Completed,
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    public Dictionary<int, QuestData> quests = new Dictionary<int, QuestData>();
    private Dictionary<int, QuestStatus> questStatuses = new Dictionary<int, QuestStatus>();

    private static string logPrefix = "[QuestManager] ";

    public QuestUI questUI;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (questUI == null)
        {
            Debug.LogError(logPrefix + "QuestUI not found.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questUI.ToggleQuestUI();
        }
    }

    public void InitializeQuest(QuestData quest)
    {
        if (quest == null)
        {
            Debug.LogError(logPrefix + "퀘스트가 null입니다.");
            return;
        }

        if (!quests.ContainsKey(quest.questID))
        {
            quests.Add(quest.questID, quest);
            questStatuses.Add(quest.questID, QuestStatus.NotStarted);
        }
    }

    public void AcceptQuest(QuestData newQuest)
    {
        if (newQuest == null)
        {
            Debug.LogError(logPrefix + "퀘스트가 null입니다.");
            return;
        }

        if (!quests.ContainsKey(newQuest.questID))
        {
            InitializeQuest(newQuest);
            questStatuses[newQuest.questID] = QuestStatus.Accepted;

            Debug.Log(logPrefix + "퀘스트 추가: " + newQuest.questTitle);

            questUI.RefreshQuestDisplay();
        }
        else
        {
            Debug.Log(logPrefix + "이미 수락한 퀘스트입니다: " + newQuest.questTitle);
        }
    }

    public void CompleteQuest(int questID)
    {
        QuestData quest = quests[questID];
        if (quest != null && questStatuses[questID] == QuestStatus.Accepted)
        {
            questStatuses[questID] = QuestStatus.Completed;

            Debug.Log(logPrefix + "퀘스트 완료: " + quest.questTitle);
            questUI.RefreshQuestDisplay();

            Debug.Log(logPrefix + "퀘스트 보상 지급: " + quest.questTitle);
            InventoryManager.Instance.AddItem(quest.rewardItem);
        }
        else
        {
            Debug.Log(logPrefix + "퀘스트를 찾을 수 없거나 이미 완료되었습니다.");
        }
    }

    public QuestStatus GetQuestStatus(int questID)
    {
        if (questStatuses.ContainsKey(questID))
        {
            return questStatuses[questID];
        }
        else
        {
            return QuestStatus.NotStarted; // TODO if quest not found
        }
    }

    public QuestStatus GetQuestStatus(QuestData quest)
    {
        return GetQuestStatus(quest.questID);
    }

    public void ResetQuest(int questID)
    {
        if (quests.ContainsKey(questID))
        {
            questStatuses[questID] = QuestStatus.NotStarted;
        }
    }

    public void PrintQuests()
    {
        Debug.Log(logPrefix + "현재 활성화된 퀘스트: ");
        foreach (KeyValuePair<int, QuestData> q in quests)
        {
            QuestStatus qStatus = questStatuses[q.Key];
            Debug.Log(logPrefix + $"ID: {q.Value.questID}, 제목: {q.Value.questTitle}, 상태: {qStatus}");
        }
    }
}
