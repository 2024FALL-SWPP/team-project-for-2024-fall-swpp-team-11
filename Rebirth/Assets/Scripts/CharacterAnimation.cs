using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(Vector3 moveDir)
    {
        if (DialogueManager.Instance.IsDialogueActive)
        return;

        if (!animator) return;
        
        animator.SetFloat("MoveForward", moveDir.z);
        animator.SetFloat("MoveRight", moveDir.x);
    }
    public void PlayJumpAnimation()
    {
        if (DialogueManager.Instance.IsDialogueActive)
        return;
        
        if (!animator) return;
        
        animator.SetTrigger("Jump");
    }
}
