using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    private string currentDialogue; // 현재 대화 텍스트
    private bool isTyping = false; // 현재 타이핑 중인지 여부

    public void ShowDialogue(string dialogue)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        currentDialogue = dialogue;
        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        typingCoroutine = null;
    }

    // 현재 타이핑 중인 텍스트를 즉시 완성
    public void CompleteTyping()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue;
            isTyping = false;
        }
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
