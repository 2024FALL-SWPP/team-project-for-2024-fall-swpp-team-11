using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class SceneTransitionManager : SingletonManager<SceneTransitionManager>
{

    public Animator fadeAnimator; // FadePanel의 Animator
    public Canvas canvas; // FadePanel의 Canvas
    public float fadeDuration = 1f; // 페이드 애니메이션 길이

    private Vector3 playerTargetPosition; // 플레이어의 목표 위치 저장

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        canvas.enabled = false;
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
        Debug.Log("Transition to " + sceneName);
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
        canvas.enabled = true;
        fadeAnimator.SetTrigger("FadeInTrigger");
        StartCoroutine(DisableCanvasAfterDelay());
    }

    public void FadeOut()
    {
        canvas.enabled = true;
        fadeAnimator.SetTrigger("FadeOutTrigger");
        StartCoroutine(DisableCanvasAfterDelay());
    }
    
    private IEnumerator DisableCanvasAfterDelay()
    {
        yield return new WaitForSeconds(fadeDuration);
        canvas.enabled = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
