using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSettingUI : SingletonManager<GameSettingUI>
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject settingsPanel;
    public GameObject UserUiCanvas;

    private bool isOutGame;

    private void Start()
    {   
        isOutGame = SceneManager.GetActiveScene().name == "MainMenu"
                || SceneManager.GetActiveScene().name == "Narration"
                || SceneManager.GetActiveScene().name == "EndingScene";

        if(isOutGame){
            UserUiCanvas.SetActive(false);
        } 
        else {
            UserUiCanvas.SetActive(true);
        }
        settingsPanel.SetActive(false);
        

        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Quit 버튼 클릭 시 코루틴 시작
        quitButton.onClick.AddListener(() => StartCoroutine(OnQuitButtonClicked()));
    }

    private void Update()
    {
        isOutGame = SceneManager.GetActiveScene().name == "MainMenu"
                || SceneManager.GetActiveScene().name == "Narration"
                || SceneManager.GetActiveScene().name == "EndingScene";

        if(isOutGame){
            UserUiCanvas.SetActive(false);
        } 
        else {
            UserUiCanvas.SetActive(true);
        }
        
        // G 키 입력 감지
        if (Input.GetKeyDown(KeyCode.G))
        {
            // 설정 창 활성화/비활성화 토글
            if (settingsPanel.activeSelf)
            {
                HideSetting();
            }
            else
            {
                ShowSetting();
            }
        }
    }

    public void ShowSetting()
    {
        settingsPanel.SetActive(true);
        GameStateManager.Instance.LockView();
    }

    private void HideSetting()
    {
        settingsPanel.SetActive(false);
        GameStateManager.Instance.UnlockView();
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    /// <summary>
    /// 종료 버튼 클릭 시 호출되는 코루틴 메서드
    /// 게임을 저장하고 1초 후 애플리케이션을 종료합니다.
    /// </summary>
    private IEnumerator OnQuitButtonClicked()
    {
        // 게임 저장
        SaveManager.Instance.SaveGame();
        Debug.Log("게임이 저장되었습니다.");

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 애플리케이션 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
