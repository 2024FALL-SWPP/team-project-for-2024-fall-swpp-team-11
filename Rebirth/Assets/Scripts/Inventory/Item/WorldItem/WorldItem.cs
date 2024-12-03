using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    private Outline outline;
    public AudioClip PickingSound;
    private AudioSource audioSource;

    // 상호작용이 이미 이루어졌는지 여부를 추적하는 플래그
    private bool hasBeenInteracted = false;

    private void Awake()
    {
        outline = GetComponent<Outline>();

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
        if (!outline) return;

        outline.enabled = true;
    }

    public void OnDefocus()
    {
        if (!outline) return;

        outline.enabled = false;
    }
}
