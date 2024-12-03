using UnityEngine;
using System.Collections;

public class CharacterMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Audio Settings")]
    public AudioClip footstepSound;
    public float footstepInterval = 0.3f;
    public float footstepDelay = 0.2f; // 딜레이 시간

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isPlayingFootstep = false;
    private bool footstepDelayActive = false; // 딜레이 활성화 플래그

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D 컴포넌트를 찾을 수 없습니다.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public void Move(Vector3 moveDir)
    {
        if (!rb || moveDir == Vector3.zero)
        {
            isPlayingFootstep = false;
            if (audioSource.isPlaying) audioSource.Stop(); // 멈췄을 때 발소리 멈춤
            return;
        }

        Vector2 moveDir2D = new Vector2(moveDir.x, moveDir.y);
        Vector2 moveDirection = transform.right * moveDir2D.x + transform.up * moveDir2D.y;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        if (!isPlayingFootstep && !footstepDelayActive) // 딜레이가 없을 때만 발소리 재생
        {
            StartCoroutine(PlayFootstepSoundWithDelay());
        }
    }

    private IEnumerator PlayFootstepSoundWithDelay()
    {
        footstepDelayActive = true; // 딜레이 활성화
        yield return new WaitForSeconds(footstepDelay); // 딜레이 시간

        if (footstepSound != null && audioSource != null)
        {
            StartCoroutine(PlayFootstepSound());
        }

        footstepDelayActive = false; // 딜레이 비활성화
    }

    private IEnumerator PlayFootstepSound()
    {
        isPlayingFootstep = true;

        while (true) // 플레이어가 이동 중인 동안 발소리 반복 재생
        {
            if (footstepSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(footstepSound);
            }

            yield return new WaitForSeconds(footstepInterval);

            // 플레이어가 멈췄거나 이동 입력이 없으면 루프 종료
            if (rb.velocity == Vector2.zero)
            {
                break;
            }
        }

        isPlayingFootstep = false;
    }
}
