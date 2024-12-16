// Portal.cs
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class Portal : MonoBehaviour, IInteractable
{
    public string targetScene; // 이동할 씬 이름
    public Vector3 targetPosition;

    public float animationDelay = 0.5f; // 씬 전환 전에 지연 시간
    public AudioClip interactionSound;

    public async void Interact()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            if (SceneTransitionManager.Instance != null)
            {
                if (interactionSound != null)
                {
                    AudioSource.PlayClipAtPoint(interactionSound, transform.position);
                }
                
                await InteractWithSceneTransitionAsync();
            }
        }
        else
        {
            Debug.LogWarning("Target scene is not set.");
        }
    }

    private async Task InteractWithSceneTransitionAsync()
    {
        DimensionManager.Instance.RefreshDimension(targetScene);
        InventoryManager.Instance.HandleSceneChange();
        CharacterStatusManager.Instance.RefreshStatusUI();

        await SceneTransitionManager.Instance.FadeInAsync();
        await SceneTransitionManager.Instance.LoadSceneAsync(targetScene);
        
        AfterSceneLoad();

        SceneTransitionManager.Instance.SetPlayerPosition(targetPosition);
        await SceneTransitionManager.Instance.FadeOutAsync();
    }

    protected virtual void AfterSceneLoad()
    {
        // 씬 로드 후 추가 작업 (선택 사항)
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