using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : SingletonManager<SceneTransitionManager>
{
    public Animator fadeAnimator;
    public Canvas canvas;
    public float fadeDuration = 1f;
    private Vector3 playerTargetPosition;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        canvas.enabled = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameStateManager.Instance.UnlockView();
        GameStateManager.Instance.UnlockMovement();
        // SetPlayerPosition();
    }

    private void SetPlayerPosition()
    {
        Transform player = GameObject.FindWithTag("Player")?.transform;
        if (player != null)
        {
            player.position = playerTargetPosition;
        }
        else
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
        }
    }

    public IEnumerator FadeInCoroutine()
    {
        canvas.enabled = true;
        fadeAnimator.SetTrigger("FadeInTrigger");
        yield return new WaitForSeconds(fadeDuration);
        canvas.enabled = false;
    }

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        GameStateManager.Instance.LockView();
        GameStateManager.Instance.LockMovement();

        SceneDataManager.Instance.SaveCurrentSceneData();
        yield return TransitionCoroutine(sceneName);
    }

    public IEnumerator FadeOutCoroutine()
    {
        canvas.enabled = true;
        fadeAnimator.SetTrigger("FadeOutTrigger");
        yield return new WaitForSeconds(fadeDuration);
        canvas.enabled = false;
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
