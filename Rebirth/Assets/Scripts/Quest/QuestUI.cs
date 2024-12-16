using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questUI;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject questPrefab;


    private List<GameObject> questItemPool = new List<GameObject>();

    private void Start()
    {
        HideQuestUI();
    }

    public void ToggleQuestUI()
    {
        if (!questUI.activeSelf)
            ShowQuestUI();
        else
            HideQuestUI();
    }

    public void ShowQuestUI()
    {
        questUI.SetActive(true);

        QuestManager.Instance.PrintQuests();

        GameStateManager.Instance.LockView();
        UIManager.Instance.AddToUIStack(questUI);
        RefreshQuestDisplay();
    }

    public void HideQuestUI()
    {
        questUI.SetActive(false);

        GameStateManager.Instance.UnlockView();
        UIManager.Instance.RemoveFromUIStack(questUI);
    }

    public void RefreshQuestDisplay()
    {
        foreach (Transform child in contentPanel)
        {
            child.gameObject.SetActive(false);
        }

        foreach (var quest in QuestManager.Instance.quests.Values)
        {
            if (quest == null)
                continue;

            if (QuestManager.Instance.GetQuestStatus(quest) == QuestStatus.Completed)
                continue;

            GameObject questObj = GetPooledQuestItem();
            questObj.transform.SetParent(contentPanel, false);
            questObj.SetActive(true);
            var questUIComponent = questObj.GetComponent<QuestItem>();
            questUIComponent.Initialize(quest);
        }
    }

    private GameObject GetPooledQuestItem()
    {
        foreach (var obj in questItemPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject newObj = Instantiate(questPrefab);
        questItemPool.Add(newObj);
        return newObj;
    }
}
