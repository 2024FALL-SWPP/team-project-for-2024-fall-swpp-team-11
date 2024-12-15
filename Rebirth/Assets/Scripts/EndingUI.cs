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

    public List<GameObject> EndingFourImages; 
    public List<TextMeshProUGUI> EndingFourTexts; 
    public NPC EndingFourNPC;

    public AudioSource audioSource;
    public AudioClip eventMusic1;
    public AudioClip eventMusic2;
    public AudioClip eventMusic3;
    public AudioClip eventMusic4;
    public AudioClip eventMusic5;

    public Button ToMainMenu;

    public float typingSpeed = 0.05f; 

    void Start()
    {
        Debug.Log(CharacterStatusManager.Instance.EndingID);
        GameStateManager.Instance.LockView();

        if(CharacterStatusManager.Instance.EndingID == 1) { // Victory 
            audioSource.clip = eventMusic1; 
            CharacterStatusManager.Instance.SetPlayerState(PlayerState.CanUseWeirdPotion);
            StartCoroutine(EnableImagesSequentially(EndingOneImages, EndingOneTexts, EndingOneNPC));
        }
        else if(CharacterStatusManager.Instance.EndingID == 2) { // Happy Ending?
            audioSource.clip = eventMusic2; 
            CharacterStatusManager.Instance.SetPlayerState(PlayerState.CanUseWeirdPotionCure);
            StartCoroutine(EnableImagesSequentially(EndingTwoImages, EndingTwoTexts, EndingTwoNPC));
        }
        else if(CharacterStatusManager.Instance.EndingID == 3) { // Truth
            audioSource.clip = eventMusic3; 
            StartCoroutine(EnableImagesSequentiallyAndHideText(EndingThreeImages, EndingThreeTexts, EndingThreeNPC));
        }
        else if(CharacterStatusManager.Instance.EndingID == 4) { // Happily Ever After..
            audioSource.clip = eventMusic4; 
            StartCoroutine(EnableImagesSequentiallyAndHideText(EndingFourImages, EndingFourTexts, EndingFourNPC));
        }
        else if(CharacterStatusManager.Instance.EndingID == 5) { // Victory with WeirdPotion
            audioSource.clip = eventMusic5; 
            StartCoroutine(EnableImagesSequentially(EndingOneImages, EndingOneTexts, EndingFourNPC));
        }
        DiskSaveSystem.ResetFilesExceptPlayerState();
        SaveManager.Instance.LoadGame();
    }
    

    IEnumerator EnableImagesSequentially(List<GameObject> images, List<TextMeshProUGUI> texts, NPC npc)
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
                yield return StartCoroutine(TypeText(texts[i], texts[i].text)); 
            }

            yield return new WaitForSeconds(1f); 
        }

        if (npc){
            npc.Interact();
        }

        
        ToMainMenu.gameObject.SetActive(true);
        GameStateManager.Instance.LockView();
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

        
        ToMainMenu.gameObject.SetActive(true);
        GameStateManager.Instance.LockView();
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
