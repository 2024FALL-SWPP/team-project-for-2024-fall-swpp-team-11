using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questUI;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject questPrefab;
    
    private bool isVisible = false;

    private List<GameObject> questItemPool = new List<GameObject>();

    private void Start()
    {
        HideQuestUI();
    }

    public void ToggleQuestUI()
    {
        if (!isVisible)
            ShowQuestUI();
        else
            HideQuestUI();
    }

    public void ShowQuestUI()
    {
        isVisible = true;
        questUI.SetActive(true);

        QuestManager.Instance.PrintActiveQuests();
        
        GameStateManager.Instance.LockView();
        RefreshQuestDisplay();
    }

    public void HideQuestUI()
    {
        isVisible = false;
        questUI.SetActive(false);
        
        GameStateManager.Instance.UnlockView();
    }

    public void RefreshQuestDisplay()
    {
        foreach (Transform child in contentPanel)
        {
            child.gameObject.SetActive(false);
        }

        foreach (var quest in QuestManager.Instance.activeQuests.Values)
        {
            GameObject questObj = GetPooledQuestItem();
            questObj.transform.SetParent(contentPanel, false);
            questObj.SetActive(true);
            var questUIComponent = questObj.GetComponent<QuestItemUI>();
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
