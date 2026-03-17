using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float textSpeed = 0.03f;

    private string[] dialogueLines;
    private int index;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLine;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.dialogueActive)
            return;

        if (GameManager.Instance.pauseActive)
            return;

        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }

    public void StartDialogue(TextoID dialogueFile)
    {
        dialogueLines = UITextFileLoader.LoadLinesFromFile(dialogueFile);

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning($"No dialogue lines found for {dialogueFile}");
            return;
        }

        index = 0;
        dialogueText.text = "";

        GameManager.Instance.SetDialogueActive(true);
        ShowLine(dialogueLines[index]);
    }

    private void NextLine()
    {
        if (GameManager.Instance.pauseActive)
            return;

        if (isTyping)
        {
            CompleteCurrentLine();
            return;
        }

        index++;

        if (index >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine(dialogueLines[index]);
    }

    private void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = currentLine;
        isTyping = false;
    }

    private void ShowLine(string line)
    {
        currentLine = line;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;
        dialogueText.text = "";
        GameManager.Instance.SetDialogueActive(false);
    }
}