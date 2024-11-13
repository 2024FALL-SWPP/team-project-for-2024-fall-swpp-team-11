using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;
    [SerializeField] private QuestUI questUI;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questUI.ToggleQuestUI();
        }
    }
}
