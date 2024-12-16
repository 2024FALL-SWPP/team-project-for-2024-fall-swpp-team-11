using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    [Serializable]
    public class MenuPanel
    {
        public GameObject panel;
        public string panelName;
        public UnityEvent onOpen;
        public UnityEvent onClose;
    }

    [Header("Scene Management")]
    [SerializeField] private bool useLoadingScreen;
    [SerializeField] private string loadingSceneName = "LoadingScene";

    [Header("Menu Panels")]
    [SerializeField] private MenuPanel[] menuPanels;

    private string gameSceneName = "Narration";
    private MenuPanel currentPanel;

    private void Awake()
    {
        SaveManager.load += LoadStartSceneData;
     
        GameStateManager.Instance.LockView();

        CloseAllPanels();
    }

    private void Start()
    {
        GameStateManager.Instance.LockView();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllPanels();
        }
    }

    #region Scene Management
    public async void StartGame()
    {
        if (useLoadingScreen)
        {
            StartCoroutine(LoadSceneAsync(gameSceneName));
        }
        else
        {
            DimensionManager.Instance.RefreshDimension(gameSceneName);
            await SceneTransitionManager.Instance.SceneTransitionWithEffect(gameSceneName);
        }
    }

    public void RestartGame()
    {
        // Restart Logic
        DiskSaveSystem.ResetAllFiles();
        SaveManager.Instance.LoadGame();
        
        // StartGame
        StartGame();
    }

    private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(loadingSceneName);
        while (!loadingScene.isDone)
        {
            yield return null;
        }

        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!sceneLoad.isDone)
        {
            float progress = Mathf.Clamp01(sceneLoad.progress / 0.9f);
            yield return null;
        }
    }
    #endregion

    #region Panel Management
    public void OpenPanel(string panelName)
    {
        MenuPanel targetPanel = Array.Find(menuPanels, panel => panel.panelName == panelName);

        if (targetPanel == null)
        {
            Debug.LogWarning($"Panel '{panelName}' not found!");
            return;
        }

        if (currentPanel != null)
        {
            CloseCurrentPanel();
        }

        targetPanel.panel.SetActive(true);
        targetPanel.onOpen?.Invoke();
        currentPanel = targetPanel;
    }

    public void CloseCurrentPanel()
    {
        if (currentPanel != null)
        {
            currentPanel.panel.SetActive(false);
            currentPanel.onClose?.Invoke();
            currentPanel = null;
        }
    }

    public void CloseAllPanels()
    {
        foreach (var menuPanel in menuPanels)
        {
            menuPanel.panel.SetActive(false);
        }
        currentPanel = null;
    }
    #endregion

    #region Load Data
    public void LoadStartSceneData()
    {
        CharacterStatusData data = DiskSaveSystem.LoadCharacterStatusFromDisk();
        gameSceneName = data.LastScene;
    }

    #endregion

    #region Application Management
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    #endregion
}