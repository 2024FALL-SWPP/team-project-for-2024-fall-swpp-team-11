using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject questUI;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject questPrefab;

    public static QuestUI Instance { get; private set; }
    
    private bool isVisible = false;
    private QuestManager questManager;

    private List<GameObject> questItemPool = new List<GameObject>();

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
    }

    private void Start()
    {
        questManager = QuestManager.Instance;
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
        questManager.PrintActiveQuests();
        isVisible = true;
        questUI.SetActive(true);
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

        foreach (var quest in questManager.activeQuests)
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
