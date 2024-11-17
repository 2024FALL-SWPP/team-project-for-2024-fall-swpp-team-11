using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    public Dictionary<int, QuestData> activeQuests = new Dictionary<int, QuestData>();

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
            Debug.LogError("QuestUI not found.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questUI.ToggleQuestUI();
        }
    }

    public void AddQuest(QuestData newQuest)
    {
        Debug.Log($"AddQuest 호출됨: {newQuest.questTitle}", this);
        if (newQuest == null)
        {
            Debug.LogError("퀘스트가 null입니다.");
            return;
        }

        if (!activeQuests.ContainsKey(newQuest.questID))
        {
            activeQuests.Add(newQuest.questID, newQuest);
            Debug.Log("퀘스트 추가: " + newQuest.questTitle);

            questUI.RefreshQuestDisplay();
        }
        else
        {
            Debug.Log("이미 수락한 퀘스트입니다: " + newQuest.questTitle);
        }
    }

    public void CompleteQuest(int questID)
    {
        QuestData quest = activeQuests[questID];
        if (quest != null && !quest.isCompleted)
        {
            quest.isCompleted = true;
            Debug.Log("퀘스트 완료: " + quest.questTitle);
            // TODO: 보상 지급 로직 추가

            questUI.RefreshQuestDisplay();
        }
        else
        {
            Debug.Log("퀘스트를 찾을 수 없거나 이미 완료되었습니다.");
        }
    }

    public void PrintActiveQuests()
    {
        Debug.Log("현재 활성화된 퀘스트: ");
        foreach (KeyValuePair<int, QuestData> q in activeQuests)
        {
            Debug.Log($"ID: {q.Value.questID}, 제목: {q.Value.questTitle}, 완료 여부: {q.Value.isCompleted}");
        }
    }
}
