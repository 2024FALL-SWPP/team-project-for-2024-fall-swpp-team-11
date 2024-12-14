using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;  

public class EndingUI : MonoBehaviour
{
    public List<GameObject> EndingOneImages; 
    public List<TextMeshProUGUI> EndingOneTexts; 
    public NPC EndingOneNPC;

    public List<GameObject> EndingTwoImages; 
    public List<TextMeshProUGUI> EndingTwoTexts; 
    public NPC EndingTwoNPC;

    public List<GameObject> EndingThreeImages; 
    public List<TextMeshProUGUI> EndingThreeTexts; 
    public NPC EndingThreeNPC;

    public AudioSource audioSource;
    public AudioClip eventMusic;

    public float typingSpeed = 0.05f; 

    void Start()
    {
        GameStateManager.Instance.LockView();
        if(CharacterStatusManager.Instance.EndingID == 1) {
            StartCoroutine(EnableImagesSequentially(EndingOneImages, EndingOneTexts, EndingOneNPC));
        }
        else if(CharacterStatusManager.Instance.EndingID == 2) {
            StartCoroutine(EnableImagesSequentially(EndingTwoImages, EndingTwoTexts, EndingTwoNPC));
        }
        else if(CharacterStatusManager.Instance.EndingID == 0) {
            StartCoroutine(EnableImagesSequentiallyAndHideText(EndingThreeImages, EndingThreeTexts, EndingThreeNPC));
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

    IEnumerator EnableImagesSequentiallyAndHideText(List<GameObject> images, List<TextMeshProUGUI> texts, NPC npc)
    {
        int count = Mathf.Min(images.Count, texts.Count);
        audioSource.Play(); 
        for (int i = 0; i < count; i++)
        {
            if (images[i] != null)
            {
                images[i].SetActive(true);
            }

            if (texts[i] != null)
            {
                if(i!=0) {
                    texts[i-1].gameObject.SetActive(false); 
                }
                yield return StartCoroutine(TypeText(texts[i], texts[i].text)); 
            }

            yield return new WaitForSeconds(2f); 
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
