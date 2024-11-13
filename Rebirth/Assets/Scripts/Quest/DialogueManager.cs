using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel; // 대화 패널 UI
    public TextMeshProUGUI dialogueText; // 대화 텍스트 UI
    public float textSpeed = 0.05f; // 텍스트 출력 속도

    // YesButton과 NoButton을 직접 참조
    public Button yesButton; // 퀘스트 수락 버튼
    public Button noButton; // 퀘스트 거절 버튼

    private Queue<string> sentences; // 대화의 문장들을 저장할 큐
    private bool isDisplaying = false;
    private Coroutine typingCoroutine; // 현재 실행 중인 타이핑 코루틴
    private string currentSentence = ""; // 현재 타이핑 중인 문장

    public event Action<bool> OnDialogueEnded; // 대화 종료 이벤트

    private Action acceptCallback;
    private Action rejectCallback;

    public bool IsDialogueActive => dialoguePanel.activeSelf;

    // 대화 상태를 관리하기 위한 enum
    private enum DialoguePhase
    {
        Talking,
        SelectingQuest
    }

    private DialoguePhase currentPhase = DialoguePhase.Talking;

    // 추가 변수
    private int selectedOption = 0; // 0: Yes, 1: No
    private Button[] questOptions; // 퀘스트 옵션 버튼 배열
    private Image[] questOptionImages; // 퀘스트 옵션 Image 배열

    // 선택된 색상과 기본 색상 정의
    public Color selectedColor = Color.yellow; // 선택 시 노란색
    public Color unselectedColor = Color.white; // 기본 색상

    private void Awake()
    {
        // 싱글톤 패턴 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
        // 버튼들을 비활성화
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);

        // 버튼 배열 초기화
        questOptions = new Button[] { yesButton, noButton };

        // Image 컴포넌트 배열 초기화
        questOptionImages = new Image[questOptions.Length];
        for (int i = 0; i < questOptions.Length; i++)
        {
            questOptionImages[i] = questOptions[i].GetComponent<Image>();
            if (questOptionImages[i] == null)
            {
                Debug.LogError($"Button at index {i} does not have an Image component.");
            }
        }

        // 초기 선택 설정
        selectedOption = 0;
        UpdateButtonSelection();
    }

    // 대화 시작
    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        sentences.Clear();

        foreach (string line in dialogue.lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    // 다음 문장 표시
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    // 문장을 타이핑 효과로 표시
    IEnumerator TypeSentence(string sentence)
    {
        isDisplaying = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isDisplaying = false;
    }

    // 대화 종료
    public void EndDialogue()
    {
        // 마지막 대화 문장 숨기기
        dialogueText.text = "";
        // 퀘스트 옵션 버튼들을 표시
        ShowQuestOptions(() => HandleQuestDecision(true), () => HandleQuestDecision(false));

        // 대화 상태를 선택 상태로 변경
        currentPhase = DialoguePhase.SelectingQuest;
    }

    // 퀘스트 옵션 처리 메서드
    private void HandleQuestDecision(bool accepted)
    {
        OnDialogueEnded?.Invoke(accepted);
        HideQuestOptions();

        // 대화 상태를 초기화
        currentPhase = DialoguePhase.Talking;

        dialoguePanel.SetActive(false);
    }

    // 퀘스트 옵션 표시
    public void ShowQuestOptions(Action onAccept, Action onReject)
    {
        Debug.Log("퀘스트 옵션 표시");
        acceptCallback = onAccept;
        rejectCallback = onReject;
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

        // 선택 인덱스 초기화 및 업데이트
        selectedOption = 0;
        UpdateButtonSelection();
    }

    // 퀘스트 옵션 숨기기
    public void HideQuestOptions()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        acceptCallback = null;
        rejectCallback = null;
    }

    // 버튼 클릭 시 호출되는 메서드
    private void OnYesButtonClicked()
    {
        Debug.Log("Yes 버튼 클릭됨");
        acceptCallback?.Invoke();
        HandleQuestDecision(true); // 퀘스트 수락 처리
    }

    private void OnNoButtonClicked()
    {
        Debug.Log("No 버튼 클릭됨");
        rejectCallback?.Invoke();
        HandleQuestDecision(false); // 퀘스트 거절 처리
    }

    // 선택된 버튼 업데이트
    private void UpdateButtonSelection()
    {
        Debug.Log($"현재 선택된 옵션: {selectedOption}");
        for (int i = 0; i < questOptions.Length; i++)
        {
            if (questOptionImages[i] != null)
            {
                if (i == selectedOption)
                {
                    questOptionImages[i].color = selectedColor; // 선택 시 색상 변경
                }
                else
                {
                    questOptionImages[i].color = unselectedColor; // 기본 색상
                }
            }

            // Outline 컴포넌트를 사용하여 선택 상태 표시 (선택 사항)
            Outline outline = questOptions[i].GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = (i == selectedOption);
            }
        }
    }

    // 방향키 입력 처리
    private void HandleQuestOptionNavigation()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedOption = (selectedOption + 1) % questOptions.Length;
            UpdateButtonSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedOption = (selectedOption - 1 + questOptions.Length) % questOptions.Length;
            UpdateButtonSelection();
        }
    }

    // 스페이스바 입력 처리
    private void HandleQuestOptionSelection()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedOption == 0)
            {
                OnYesButtonClicked();
            }
            else if (selectedOption == 1)
            {
                OnNoButtonClicked();
            }
        }
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf)
        {
            if (currentPhase == DialoguePhase.Talking)
            {
                if (isDisplaying)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                    {
                        // 현재 타이핑 중인 문장을 즉시 완성
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }
                        dialogueText.text = currentSentence;
                        isDisplaying = false;
                    }
                }
                else
                {
                    // 다음 문장으로 넘어감
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                    {
                        DisplayNextSentence();
                    }
                }
            }
            else if (currentPhase == DialoguePhase.SelectingQuest)
            {
                // 퀘스트 옵션이 표시된 경우 네비게이션 및 선택 처리
                HandleQuestOptionNavigation();
                HandleQuestOptionSelection();
            }
        }
    }
}
