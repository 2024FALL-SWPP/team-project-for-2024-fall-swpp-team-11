using UnityEngine;
using System.Collections;

public class CharacterMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Audio Settings")]
    public AudioClip footstepSound;
    public float footstepInterval = 0.3f;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isPlayingFootstep = false;

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
            return;
        }

        Vector2 moveDir2D = new Vector2(moveDir.x, moveDir.y);
        Vector2 moveDirection = transform.right * moveDir2D.x + transform.up * moveDir2D.y;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        if (!isPlayingFootstep)
        {
            StartCoroutine(PlayFootstepSound());
        }
    }

    private IEnumerator PlayFootstepSound()
    {
        isPlayingFootstep = true;

        if (footstepSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(footstepSound);
        }

        yield return new WaitForSeconds(footstepInterval);
        isPlayingFootstep = false;
    }
}
