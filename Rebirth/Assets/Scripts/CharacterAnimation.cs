using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(Vector3 direction)
    {
        float speed = direction.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
