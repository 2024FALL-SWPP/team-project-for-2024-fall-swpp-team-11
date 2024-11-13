using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    
    public List<QuestData> activeQuests = new List<QuestData>();

    private QuestUI questUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        questUI = FindObjectOfType<QuestUI>();
    }

    public void AddQuest(QuestData newQuest)
    {
        Debug.Log($"AddQuest 호출됨: {newQuest.questTitle}", this);
        if (!activeQuests.Exists(q => q.questID == newQuest.questID))
        {
            activeQuests.Add(newQuest);
            Debug.Log("퀘스트 추가: " + newQuest.questTitle);
            questUI?.RefreshQuestDisplay();
            // 추가적인 UI 업데이트 로직
        }
        else
        {
            Debug.Log("이미 수락한 퀘스트입니다: " + newQuest.questTitle);
        }
    }

    public void CompleteQuest(int questID)
    {
        QuestData quest = activeQuests.Find(q => q.questID == questID);
        if (quest != null && !quest.isCompleted)
        {
            quest.isCompleted = true;
            Debug.Log("퀘스트 완료: " + quest.questTitle);
            // 보상 지급 로직 추가
            questUI?.RefreshQuestDisplay();
        }
        else
        {
            Debug.Log("퀘스트를 찾을 수 없거나 이미 완료되었습니다.");
        }
    }

    public void PrintActiveQuests()
    {
        Debug.Log("현재 활성화된 퀘스트: ");
        foreach (QuestData q in activeQuests)
        {
            Debug.Log($"ID: {q.questID}, 제목: {q.questTitle}, 완료 여부: {q.isCompleted}");
        }
    }
}
