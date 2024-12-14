using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;  

public class EndingUI : MonoBehaviour
{
    public TextMeshProUGUI uiText; 
    public List<GameObject> EndingOneImages; 
    public List<TextMeshProUGUI> EndingOneTexts; 
    public NPC EndingOneNPC;
    public AudioSource audioSource;
    public AudioClip eventMusic;

    public float typingSpeed = 0.05f; 

    void Start()
    {
        GameStateManager.Instance.LockView();
        if(CharacterStatusManager.Instance.EndingID == 0) {
            StartCoroutine(EnableImagesSequentially(EndingOneImages, EndingOneTexts, EndingOneNPC));
        }
    }

    IEnumerator EnableImagesSequentially(List<GameObject> images, List<TextMeshProUGUI> texts, NPC npc)
    {
        int count = Mathf.Min(images.Count, texts.Count);
        audioSource.Play(); 
        for (int i = 0; i < count; i++)
        {
            // if(i==1) audioSource.Play(); 
            if (images[i] != null)
            {
                images[i].SetActive(true);
            }

            if (texts[i] != null)
            {
                yield return StartCoroutine(TypeText(texts[i], texts[i].text)); 
            }

            yield return new WaitForSeconds(1f); 
        }

        if (npc){
            npc.Interact();
        }
    }

    IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = ""; 
        textComponent.gameObject.SetActive(true); 
        foreach (char letter in fullText)
        {
            textComponent.text += letter; 
            yield return new WaitForSeconds(typingSpeed); 
        }
    }

}
