using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DimensionManager : SingletonManager<DimensionManager>
{
    public GameObject player2DPrefab;
    public GameObject player3DPrefab;

    private string anchorID;
    private bool isSwitching = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isSwitching)
        {
            StartCoroutine(SwitchDimension());
        }
    }

    IEnumerator SwitchDimension()
    {
        isSwitching = true;

        // 현재 씬 이름 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 대상 씬 이름 결정 (마지막 두 글자를 교체)
        string targetSceneName;
        if (currentSceneName.EndsWith("2D"))
        {
            targetSceneName = currentSceneName.Substring(0, currentSceneName.Length - 2) + "3D";
        }
        else if (currentSceneName.EndsWith("3D"))
        {
            targetSceneName = currentSceneName.Substring(0, currentSceneName.Length - 2) + "2D";
        }
        else
        {
            Debug.LogError("현재 씬 이름이 '2D' 또는 '3D'로 끝나지 않습니다. 차원 전환 불가.");
            isSwitching = false;
            yield break;
        }

        // 현재 플레이어 찾기
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayer == null)
        {
            Debug.LogError("현재 씬에서 플레이어를 찾을 수 없습니다.");
            isSwitching = false;
            yield break;
        }

        // 현재 씬에서 가장 가까운 앵커 찾기
        Anchor currentAnchor = FindClosestAnchor(currentPlayer.transform.position);

        if (currentAnchor == null)
        {
            Debug.LogError("현재 씬에서 앵커를 찾을 수 없습니다.");
            isSwitching = false;
            yield break;
        }

        // 앵커 ID 저장
        anchorID = currentAnchor.anchorID;

        // 움직임 및 시야 잠금
        GameStateManager.Instance.LockView();
        GameStateManager.Instance.LockMovement();

        // 페이드 인
        SceneTransitionManager.Instance.FadeIn();
        yield return new WaitForSeconds(SceneTransitionManager.Instance.fadeDuration);

        // 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // 한 프레임 대기하여 씬이 완전히 로드되도록 함
        yield return null;

        // 대상 씬에서 대응되는 앵커 찾기
        Anchor targetAnchor = FindAnchorByID(anchorID);

        if (targetAnchor == null)
        {
            Debug.LogError("대상 씬에서 대응되는 앵커를 찾을 수 없습니다.");
            isSwitching = false;
            yield break;
        }

        // 플레이어 인스턴스화 또는 기존 플레이어 찾기
        GameObject playerPrefab = GetPlayerPrefabForScene(targetSceneName);

        if (playerPrefab == null)
        {
            Debug.LogError("씬에 대한 플레이어 프리팹을 찾을 수 없습니다: " + targetSceneName);
            isSwitching = false;
            yield break;
        }

        // 플레이어가 씬에 미리 배치되어 있는 경우
        GameObject newPlayer = GameObject.FindWithTag("Player");
        if (newPlayer == null)
        {
            // 플레이어 인스턴스화
            newPlayer = Instantiate(playerPrefab, targetAnchor.transform.position, Quaternion.identity);
        }
        else
        {
            // 플레이어 위치 업데이트
            newPlayer.transform.position = targetAnchor.transform.position;
        }

        // 페이드 아웃
        SceneTransitionManager.Instance.FadeOut();

        // 움직임 및 시야 잠금 해제
        GameStateManager.Instance.UnlockView();
        GameStateManager.Instance.UnlockMovement();

        isSwitching = false;
    }

    Anchor FindClosestAnchor(Vector3 position)
    {
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        if (anchors.Length == 0)
            return null;

        Anchor closestAnchor = null;
        float closestDistance = Mathf.Infinity;

        foreach (Anchor anchor in anchors)
        {
            float distance = Vector3.Distance(position, anchor.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAnchor = anchor;
            }
        }

        return closestAnchor;
    }

    Anchor FindAnchorByID(string anchorID)
    {
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        foreach (Anchor anchor in anchors)
        {
            if (anchor.anchorID == anchorID)
            {
                return anchor;
            }
        }

        return null;
    }

    GameObject GetPlayerPrefabForScene(string sceneName)
    {
        if (sceneName.EndsWith("2D"))
        {
            return player2DPrefab;
        }
        else if (sceneName.EndsWith("3D"))
        {
            return player3DPrefab;
        }

        return null;
    }
}
