// DoorTrigger.cs
using UnityEngine;
using System.Collections;

public class PortalTransition : MonoBehaviour, IInteractable
{
    public string targetScene; // 이동할 씬 이름
    public Vector3 targetPosition;

    public float animationDelay = 0.5f; // 씬 전환 전에 지연 시간

    public void Interact()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.LoadScene(targetScene, targetPosition);
            }
            else
            {
                Debug.LogError("SceneTransitionManager.Instance가 null입니다.");
            }
        }
        else
        {
            // 같은 씬 내 위치 이동
            Transform player = GameObject.FindWithTag("Player").transform;
            StartCoroutine(MovePlayerAfterDelay(player, targetPosition, animationDelay));
        }
    }

    public virtual void OnFocus()
    {
        // UI 업데이트 등 상호작용 가능 상태 표시 (선택 사항)
    }

    public virtual void OnDefocus()
    {
        // UI 업데이트 등 상호작용 불가능 상태 표시 (선택 사항)
    }

    private IEnumerator MovePlayerAfterDelay(Transform player, Vector3 newPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.position = newPosition;
    }
}
