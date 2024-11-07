using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<Quest> activeQuests = new List<Quest>();

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
        }
    }

    public void AddQuest(Quest newQuest)
    {
        if (!activeQuests.Exists(q => q.id == newQuest.id))
        {
            activeQuests.Add(newQuest);
            Debug.Log("퀘스트 추가: " + newQuest.title);
            // UI 업데이트 추가
        }
        else
        {
            Debug.Log("이미 수락한 퀘스트입니다: " + newQuest.title);
        }
    }

    public void CompleteQuest(int questId)
    {
        Quest quest = activeQuests.Find(q => q.id == questId);
        if (quest != null && !quest.isCompleted)
        {
            quest.isCompleted = true;
            Debug.Log("퀘스트 완료: " + quest.title);
            // TODO: 아이템, 능력 부여 로직 추가 (인벤토리 로직 구현 후)
        }
        else
        {
            Debug.Log("퀘스트를 찾을 수 없거나 이미 완료되었습니다.");
        }
    }

    public void PrintActiveQuests()
    {
        Debug.Log("현재 활성화된 퀘스트: ");
        foreach (Quest q in activeQuests)
        {
            Debug.Log($"ID: {q.id}, 제목: {q.title}, 완료 여부: {q.isCompleted}");
        }
    }
}
