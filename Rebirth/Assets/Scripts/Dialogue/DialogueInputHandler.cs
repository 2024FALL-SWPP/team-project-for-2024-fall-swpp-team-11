using UnityEngine;
using System.Collections;

public class DialogueInputHandler : MonoBehaviour
{
    private bool isDialogueActive = false;
    private bool canProcessInput = true;
    private float debounceTime = 0.2f;
    private float lastInputTime = 0f;

    private static string logPrefix = "[DialogueInputHandler] ";

    private void Start()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStart += HandleDialogueStart;
            DialogueManager.Instance.OnDialogueEnd += HandleDialogueEnd;
        }
        else
        {
            Debug.LogWarning(logPrefix + "DialogueManager not found.");
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
        if (!isDialogueActive || !canProcessInput)
            return;

        if (Time.time - lastInputTime < debounceTime)
            return;

        // 스페이스바 입력 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DialogueManager.Instance.dialogueUI.dialogueTyper.IsTyping())
            {
                DialogueManager.Instance.dialogueUI.dialogueTyper.CompleteTyping();
            }
            else
            {
                DialogueManager.Instance.SelectCurrentOption();
            }
            OnInputChanged();
            return; // 스페이스바 입력이 처리되었으므로 추가 입력 처리하지 않음
        }

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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
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
