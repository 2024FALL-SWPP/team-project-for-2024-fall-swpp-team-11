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

    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    private List<GameObject> currentOptionButtons = new List<GameObject>();
    private int selectedOptionIndex = 0;

    private static string logPrefix = "[DialogueUI] ";

    public void ShowDialogue(DialogueNode node, int initialSelectedIndex = 0)
    {
        Debug.Log(logPrefix + "ShowDialogue " + node.dialogueText + " with " + node.options.Count + " options");
        dialoguePanel.SetActive(true);
        dialogueText.text = node.dialogueText;
        ClearOptions();

        foreach (DialogueOption option in node.options)
        {
            CreateOptionButton(option);
        }
        selectedOptionIndex = initialSelectedIndex;
        UpdateOptionSelection();

        GameStateManager.Instance.LockView();
        GameStateManager.Instance.LockMovement();
    }

    public void UpdateSelectedOption(int newSelectedIndex)
    {
        selectedOptionIndex = newSelectedIndex;
        UpdateOptionSelection();
    }

    private void UpdateOptionSelection()
    {
        for (int i = 0; i < currentOptionButtons.Count; i++)
        {
            Color color = i == selectedOptionIndex ? selectedColor : defaultColor;
            currentOptionButtons[i].GetComponent<Image>().color = color;
        }

        if (currentOptionButtons.Count > 0 && selectedOptionIndex >= 0 && selectedOptionIndex < currentOptionButtons.Count)
        {
            currentOptionButtons[selectedOptionIndex].GetComponent<Button>().Select();
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        ClearOptions();

        GameStateManager.Instance.UnlockView();
        GameStateManager.Instance.UnlockMovement();
        Debug.Log(logPrefix + "HideDialogue");
    }

    private void CreateOptionButton(DialogueOption option)
    {
        GameObject buttonObj = Instantiate(optionButtonPrefab, optionsContainer);
        Button button = buttonObj.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = option.optionText;

        // TODO space & arrows for now
        // Debug.Log("Creating option button: " + option.optionText + " option next node: " + option.nextNode.dialogueText);
        // button.onClick.AddListener(() =>
        // {
        //     Debug.Log("Option button clicked: " + option.optionText);
        //     OnOptionSelected(option);
        // });

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