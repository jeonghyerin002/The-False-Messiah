using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{

    [Header("UI요소 - Inspector에서 연결")]
    public GameObject DialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("타이핑 효과")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    private EndDialogeDataSO currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;

        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }

    }
    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            StopCoroutine(typingCoroutine);
        }
        //현재 줄의 대화 내용으로 타이필 효과 시작
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(currentText);
    }
    
    void EndDialogue()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }
    public void ShowNextLine()
    {
        currentLineIndex++;

        if(currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }    
        else
        {
            ShowCurrentLine();
        }
    }
    public void HandleNextInput()
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if(!isTyping)
        {
            ShowCurrentLine();
        }
        
    }
    public void SkipDialogue()
    {
        EndDialogue();
    }
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
    public void StartDialogue(EndDialogeDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;

        //대화 시작 준비
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        //UI 업데이트
        DialoguePanel.SetActive(true);
        
        ShowCurrentLine();
    }

    private void Start()
    {
        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);
    }
    private void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

}
