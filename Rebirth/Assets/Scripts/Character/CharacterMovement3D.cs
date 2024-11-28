using UnityEngine;
using System.Collections;

public class CharacterMovement3D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 500f;
    private Rigidbody rb;
    private bool canJump = true;

    [Header("Audio Clips")]
    public AudioClip footstepSound;
    public AudioClip jumpSound;
    public AudioClip landSound;

    private AudioSource audioSource;
    private bool isPlayingFootstep = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (rb && rb.velocity.y <= 0 && IsGrounded())
        {
            if (!canJump)
            {
                canJump = true;

                if (landSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(landSound); // 착지 소리 재생
                }
            }
        }
    }

    public void Move(Vector3 moveDir)
    {
        if (!rb || moveDir == Vector3.zero)
        {
            isPlayingFootstep = false;
            return;
        }

        Vector3 moveDirection = transform.right * moveDir.x + transform.forward * moveDir.z;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        if (!isPlayingFootstep && IsGrounded())
        {
            StartCoroutine(PlayFootstepSound());
        }
    }

    public void Turn(Quaternion viewRot)
    {
        Quaternion flatViewRot = Quaternion.Euler(0, viewRot.eulerAngles.y, 0);
        transform.rotation = flatViewRot;
    }

    public void Jump()
    {
        if (!rb) return;

        if (IsJumpable())
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (jumpSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSound); // 점프 소리 재생
            }
        }
    }

    private bool IsGrounded()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        float rayDist = 0.2f;

        return Physics.Raycast(rayOrigin, -Vector3.up, rayDist);
    }

    public bool IsJumpable()
    {
        return IsGrounded() && canJump;
    }

    private IEnumerator PlayFootstepSound()
    {
        isPlayingFootstep = true;

        if (footstepSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(footstepSound); // 발소리 재생
        }

        yield return new WaitForSeconds(0.3f); // 발소리 간격
        isPlayingFootstep = false;
    }
}
