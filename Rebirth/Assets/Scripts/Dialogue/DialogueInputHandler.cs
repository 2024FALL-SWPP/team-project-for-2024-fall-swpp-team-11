using UnityEngine;
using System.Collections;

public class DialogueInputHandler : MonoBehaviour
{
    private bool isDialogueActive = false;
    private bool canProcessInput = true;
    private float debounceTime = 0.2f;
    private float lastInputTime = 0f;

    private void Start()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart += HandleDialogueStart;
            DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
        }
        else
        {
            Debug.LogWarning("DialogueManager not found.");
            StartCoroutine(SubscribeToDialogueManager());
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart -= HandleDialogueStart;
            DialogueManager.Instance.OnDialogueEnd -= HandleDialogueEnd;
        }
    }

    private void HandleDialogueStart()
    {
        isDialogueActive = true;
    }

    private void HandleDialogueEnd()
    {
        isDialogueActive = false;
    }

    private void Update()
    {
        if (!isDialogueActive)
            return;

        if (!canProcessInput)
            return;
        
        if (Time.time - lastInputTime < debounceTime)
            return;

        HandleNavigationInput();
        HandleSelectionInput();
    }

    private void HandleNavigationInput()
    {
        bool changed = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DialogueManager.Instance.NavigateOption(-1);
            changed = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DialogueManager.Instance.NavigateOption(1);
            changed = true;
        }
        
        if (changed)
        {
            OnInputChanged();
        }
    }

    private void HandleSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Selecting current option");
            DialogueManager.Instance.SelectCurrentOption();
            OnInputChanged();
        }
    }

    private void OnInputChanged()
    {
        canProcessInput = false;
        lastInputTime = Time.time;
        Invoke(nameof(EnableInput), debounceTime);
    }

    private void EnableInput()
    {
        canProcessInput = true;
    }

    private IEnumerator SubscribeToDialogueManager()
    {
        while (DialogueManager.Instance == null)
        {
            yield return null;
        }

        DialogueManager.Instance.OnDialogueStart += HandleDialogueStart;
        DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
    }
}
