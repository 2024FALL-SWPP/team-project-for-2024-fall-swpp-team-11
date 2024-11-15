using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Transform optionsContainer;
    public GameObject optionButtonPrefab;

    private List<GameObject> currentOptionButtons = new List<GameObject>();

    public void ShowDialogue(DialogueNode node)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = node.dialogueText;
        ClearOptions();

        foreach (DialogueOption option in node.options)
        {
            if (option.AreConditionsMet())
            {
                CreateOptionButton(option);
            }
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        ClearOptions();
    }

    private void CreateOptionButton(DialogueOption option)
    {
        GameObject buttonObj = Instantiate(optionButtonPrefab, optionsContainer);
        Button button = buttonObj.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = option.optionText;

        button.onClick.AddListener(() =>
        {
            OnOptionSelected(option);
        });

        currentOptionButtons.Add(buttonObj);
    }

    private void OnOptionSelected(DialogueOption option)
    {
        DialogueManager.Instance.SelectOption(option);
    }

    private void ClearOptions()
    {
        foreach (GameObject button in currentOptionButtons)
        {
            Destroy(button);
        }

        currentOptionButtons.Clear();
    }
}