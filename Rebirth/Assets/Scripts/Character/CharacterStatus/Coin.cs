using UnityEngine;

public class Coin : MonoBehaviour, IInteractable
{
    public AudioClip PickingSound;
    private AudioSource audioSource;

    // 상호작용이 이미 이루어졌는지 여부를 추적하는 플래그
    private bool hasBeenInteracted = false;

    // 코인 획득 시 플레이어의 돈을 증가시키는 메서드 호출

    private void Awake()
    {
        // AudioSource 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 방지
        audioSource.clip = PickingSound; // AudioClip 설정
    }
    public void Interact()
    {
        if (hasBeenInteracted)
            return;

        hasBeenInteracted = true;

        CharacterStatusManager.Instance.UpdateMoney(1); // 돈 1원 추가
        audioSource.Play();

        
        Destroy(gameObject, 0.3f);
    }

    // 코인에 포커스될 때 (예: 플레이어가 근접했을 때) 호출되는 메서드
    public void OnFocus()
    {

    }

    // 코인 포커스가 해제될 때 호출되는 메서드
    public void OnDefocus()
    {

    }
}
