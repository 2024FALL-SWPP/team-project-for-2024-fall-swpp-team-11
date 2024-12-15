using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText; 
    [TextArea(3, 5)]
    public string[] tutorials; 
    [TextArea(3, 5)]
    public string[] randomDialogues; 
    private int currentTutorialIndex = 0;

    public GameObject tutorialModal;

    void Start()
    {
        if (tutorialModal != null)
        {
            tutorialModal.SetActive(false);
            if(CharacterStatusManager.Instance.PlayerState ==  PlayerState.Default){
                tutorialModal.SetActive(true);
            } 
        }

        if (tutorials.Length > 0)
        {
            tutorialText.text = tutorials[0];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(CharacterStatusManager.Instance.PlayerState ==  PlayerState.Default){
                ShowNextDialogue();      
            } 
            else {
                if(tutorialModal.activeSelf){
                    tutorialModal.SetActive(false);
                }
                else {
                    tutorialModal.SetActive(true);
                    ShowRandomDialogue();
                }
                
            }
            
        }
    }

    void ShowNextDialogue()
    {
        if (currentTutorialIndex < tutorials.Length - 1)
        {
            currentTutorialIndex++;
            tutorialText.text = tutorials[currentTutorialIndex];
        }
        else
        {
            Debug.Log("튜토리얼이 끝났습니다.");
            tutorialText.text = "";
            if (tutorialModal != null)
            {
                tutorialModal.SetActive(false);
            }
        }
    }

    void ShowRandomDialogue()
    {
        if (randomDialogues.Length > 0)
        {
            string randomDialogue = randomDialogues[Random.Range(0, randomDialogues.Length)]; 
            if (tutorialModal != null)
            {
                tutorialModal.SetActive(true); 
            }
            tutorialText.text = randomDialogue;
        }
        else
        {
            Debug.Log("랜덤 대사가 없습니다.");
        }
    }
}