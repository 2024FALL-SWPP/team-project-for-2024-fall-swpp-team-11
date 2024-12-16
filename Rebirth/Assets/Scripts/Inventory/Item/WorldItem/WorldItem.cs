using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    private Outline outline;
    public AudioClip PickingSound;
    private AudioSource audioSource;
    private InteractableObject2D interactable2D; // 2D용 InteractableObject2D를 참조

    // 상호작용이 이미 이루어졌는지 여부를 추적하는 플래그
    private bool hasBeenInteracted = false;

    private void Awake()
    {
        // 3D용 Outline
        outline = GetComponent<Outline>();

        // 2D용 InteractableObject2D 컴포넌트 획득 (없을 수도 있으니 null 체크 필요)
        interactable2D = GetComponent<InteractableObject2D>();

        // AudioSource 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 방지
        audioSource.clip = PickingSound; // AudioClip 설정
    }

    public void Interact()
    {
        if (!itemData) return;

        if (hasBeenInteracted)
            return;

        hasBeenInteracted = true;

        if (audioSource && PickingSound)
        {
            audioSource.Play();
        }

        InventoryManager.Instance.AddItem(itemData);
        Destroy(gameObject, 0.3f);
    }

    public void OnFocus()
    {
        // Dimension 비교 시 == 사용
        if (itemData.dimension == Dimension.THREE_DIMENSION)
        {
            // 3D일 경우 기존 outline 사용
            if (!outline) return;
            outline.enabled = true;
        }
        else
        {
            // 2D일 경우 InteractableObject2D의 OnFocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnFocus();
            }
        }
    }

    public void OnDefocus()
    {
        if (itemData.dimension == Dimension.THREE_DIMENSION)
        {
            // 3D일 경우 기존 outline 사용
            if (!outline) return;
            outline.enabled = false;
        }
        else
        {
            // 2D일 경우 InteractableObject2D의 OnDefocus 호출
            if (interactable2D != null)
            {
                interactable2D.OnDefocus();
            }
        }
    }
}
