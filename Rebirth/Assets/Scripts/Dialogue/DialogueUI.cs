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

    private void Update()
    {
        if (!dialoguePanel.activeSelf)
        {
            return;
        }

        HandleNavigationInput();
        HandleSelectionInput();
    }

    private void HandleNavigationInput()
    {
        bool changed = false;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedOptionIndex = (selectedOptionIndex - 1 + currentOptionButtons.Count) % currentOptionButtons.Count;
            changed = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedOptionIndex = (selectedOptionIndex + 1) % currentOptionButtons.Count;
            changed = true;
        }

        if (changed)
        {
            UpdateSelectedOption();
        }
    }

    private void UpdateSelectedOption()
    {
        for (int i = 0; i < currentOptionButtons.Count; i++)
        {
            ColorBlock colors = currentOptionButtons[i].GetComponent<Button>().colors;
            if (i == selectedOptionIndex)
            {
                colors.normalColor = selectedColor;
            }
            else
            {
                colors.normalColor = defaultColor;
            }
            currentOptionButtons[i].GetComponent<Button>().colors = colors;
        }

        if (currentOptionButtons.Count > 0)
        {
            currentOptionButtons[selectedOptionIndex].GetComponent<Button>().Select();
        }
    }

    private void HandleSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (currentOptionButtons.Count == 0)
            {
                // No options, end dialogue
                DialogueManager.Instance.SelectOption(null); // TODO better dependency handling
                return;
            }

            Button button = currentOptionButtons[selectedOptionIndex].GetComponent<Button>();
            button.onClick.Invoke();
        }
    }

    public void ShowDialogue(DialogueNode node)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = node.dialogueText;
        ClearOptions();

        foreach (DialogueOption option in node.options)
        {
            CreateOptionButton(option);
        }
        selectedOptionIndex = 0;
        UpdateSelectedOption();

        GameStateManager.Instance.LockView();
        Debug.Log("ShowDialogue");
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        ClearOptions();

        GameStateManager.Instance.UnlockView();
        Debug.Log("HideDialogue");
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