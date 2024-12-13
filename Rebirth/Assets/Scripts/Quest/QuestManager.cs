using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus
{
    NotStarted,
    Accepted,
    Completed,
}

public class QuestManager : SingletonManager<QuestManager>
{
    public Dictionary<int, QuestData> quests = new Dictionary<int, QuestData>();
    private Dictionary<int, QuestStatus> questStatuses = new Dictionary<int, QuestStatus>();

    private static string logPrefix = "[QuestManager] ";

    public QuestUI questUI;

    // 추가된 부분: 퀘스트 수락 시 재생할 사운드 클립
    [Header("Quest Sound Settings")]
    public AudioClip acceptQuestSound; // 인스펙터에서 설정할 수 있는 사운드 클립
    public AudioClip completeQuestSound;
    private AudioSource audioSource; // 사운드를 재생할 오디오 소스

    protected override void Awake()
    {
        base.Awake();

        SaveManager.load += LoadQuestManagerFromDisk;
        SaveManager.save += SaveQuestManagerToDisk;

        if (questUI == null)
        {
            Debug.LogError(logPrefix + "QuestUI not found.");
        }

        // AudioSource 컴포넌트 가져오기 또는 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 사운드 클립 설정 여부 확인
        if (acceptQuestSound == null)
        {
            Debug.LogWarning(logPrefix + "Accept Quest Sound is not assigned.");
        }
    }

    private void SaveQuestManagerToDisk()
    {
        DiskSaveSystem.SaveQuestManagerToDisk(quests, questStatuses);
    }

    private async void LoadQuestManagerFromDisk()
    {
        (quests, questStatuses) = await DiskSaveSystem.LoadQuestManagerFromDiskAsync();
        questUI.RefreshQuestDisplay();
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

            // 추가된 부분: 퀘스트 수락 사운드 재생
            PlayAcceptQuestSound();
        }
        else
        {
            Debug.Log(logPrefix + "이미 수락한 퀘스트입니다: " + newQuest.questTitle);
        }
    }

    private void PlayAcceptQuestSound()
    {
        if (audioSource != null && acceptQuestSound != null)
        {
            audioSource.PlayOneShot(acceptQuestSound);
        }
        else
        {
            Debug.LogWarning(logPrefix + "사운드를 재생할 준비가 되지 않았습니다.");
        }
    }

    public void CompleteQuest(int questID)
    {
        QuestData quest = quests[questID];
        if (quest != null && questStatuses[questID] == QuestStatus.Accepted)
        {
            questStatuses[questID] = QuestStatus.Completed;

            Debug.Log(logPrefix + "퀘스트 완료: " + quest.questTitle);
            CleanUpQuestRequirements(quest);
            questUI.RefreshQuestDisplay();

            Debug.Log(logPrefix + "퀘스트 보상 지급: " + quest.questTitle);
            InventoryManager.Instance.AddItem(quest.rewardItem);

            PlayCompleteQuestSound();
        }
        else
        {
            Debug.Log(logPrefix + "퀘스트를 찾을 수 없거나 이미 완료되었습니다.");
        }
    }


    private void PlayCompleteQuestSound()
    {
        if (audioSource != null && completeQuestSound != null)
        {
            audioSource.PlayOneShot(completeQuestSound);
        }
        else
        {
            Debug.LogWarning(logPrefix + "사운드를 재생할 준비가 되지 않았습니다.");
        }
    }

    private void CleanUpQuestRequirements(QuestData quest)
    {
        if (quest.requiredItem != null)
        {
            InventoryManager.Instance.RemoveItem(quest.requiredItem);
        }
        else
        {
            Debug.LogWarning(logPrefix + $"퀘스트 {quest.questTitle}에 필요한 아이템 {quest.requiredItem.itemName}이 없습니다.");
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
