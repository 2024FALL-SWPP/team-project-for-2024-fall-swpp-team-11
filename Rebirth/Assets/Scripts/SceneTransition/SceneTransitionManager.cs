using System.Threading.Tasks;
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
        canvas.enabled = false;
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

    public async Task FadeInAsync()
    {
        GameStateManager.Instance.LockView();
        GameStateManager.Instance.LockMovement();

        canvas.enabled = true;
        fadeAnimator.SetTrigger("FadeInTrigger");

        // 비동기 대기
        await Task.Delay((int)(fadeDuration * 1000));
        canvas.enabled = false;
    }

    public async Task LoadSceneAsync(string sceneName)
    {
        // 현재 씬 데이터 저장
        await SceneDataManager.Instance.SaveCurrentSceneDataAsync();

        // 씬 전환 처리
        await TransitionAsync(sceneName);
    }

    public async Task FadeOutAsync()
    {
        canvas.enabled = true;
        fadeAnimator.SetTrigger("FadeOutTrigger");

        // 비동기 대기
        await Task.Delay((int)(fadeDuration * 1000));
        canvas.enabled = false;

        GameStateManager.Instance.UnlockView();
        GameStateManager.Instance.UnlockMovement();
    }

    private async Task TransitionAsync(string sceneName)
    {
        // 비동기로 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 로드 완료 대기
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            await Task.Yield();
        }
    }
}
