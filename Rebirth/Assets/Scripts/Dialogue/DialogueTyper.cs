using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTyper : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f; 

    private Coroutine typingCoroutine;


    public void ShowDialogue(string dialogue)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }


        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = ""; 
        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null; 
    }
}

