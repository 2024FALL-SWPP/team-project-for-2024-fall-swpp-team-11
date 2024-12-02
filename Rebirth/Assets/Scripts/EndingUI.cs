using UnityEngine;
using TMPro;  

public class EndingUI : MonoBehaviour
{
    public TextMeshProUGUI uiText; 

    void Start()
    {
        UpdateText(CharacterStatusManager.Instance.EndingID.ToString());
        GameStateManager.Instance.LockView();
    }

    public void UpdateText(string newText)
    {
        if (uiText != null)
        {
            uiText.text = newText; 
        }
    }
}
