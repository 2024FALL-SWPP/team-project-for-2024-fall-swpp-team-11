// SceneTransitionManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public Animator fadeAnimator; // FadePanel의 Animator
    public float fadeDuration = 1f; // 페이드 애니메이션 길이

    private Vector3 playerTargetPosition; // 플레이어의 목표 위치 저장

    private void Awake()
    {
        // 싱글톤 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FadeOut();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameStateManager.Instance.UnlockView();
        GameStateManager.Instance.UnlockMovement();

        Transform player = GameObject.FindWithTag("Player")?.transform;
        if (player != null)
        {
            player.position = playerTargetPosition;
        }
        else
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
        }

        FadeOut();
    }

    public void LoadScene(string sceneName, Vector3 targetPosition)
    {
        playerTargetPosition = targetPosition; // 목표 위치 저장
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        GameStateManager.Instance.LockView();
        GameStateManager.Instance.LockMovement();
        FadeIn();
        yield return new WaitForSeconds(fadeDuration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void FadeIn()
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeInTrigger");
        }
    }

    public void FadeOut()
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeOutTrigger");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
