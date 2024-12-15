using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;  

public class NarrationUI : MonoBehaviour
{
    public List<TextMeshProUGUI> Texts; 

    public float typingSpeed = 0.05f; 


    void Start()
    {
        StartCoroutine(EnableImagesSequentially(Texts));
    }

     IEnumerator EnableImagesSequentially(List<TextMeshProUGUI> texts)
    {

        // audioSource.Play(); 
        for (int i = 0; i < texts.Count; i++)
        {
            if (texts[i] != null)
            {
                yield return StartCoroutine(TypeText(texts[i], texts[i].text)); 
            }

            yield return new WaitForSeconds(1f); 
        }
        
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
