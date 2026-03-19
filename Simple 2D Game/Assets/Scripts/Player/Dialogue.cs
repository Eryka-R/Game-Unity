using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float textSpeed = 0.03f;

    [Header("Characters")]
    [SerializeField] private GameObject Narrator;
    [SerializeField] private GameObject MainCharacter;
    [SerializeField] private GameObject Friend;
    [SerializeField] private GameObject Enemy1;
    [SerializeField] private GameObject Enemy2;

    private DialogueLineData[] dialogueLines;
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
        
        string[] rawLines = UITextFileLoader.LoadLinesFromFile(dialogueFile);

        if (rawLines == null || rawLines.Length == 0)
        {
            Debug.LogWarning($"No dialogue lines found for {dialogueFile}");
            return;
        }

        dialogueLines = ParseDialogueLines(rawLines);

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning($"No valid dialogue lines found for {dialogueFile}");
            return;
        }

        index = 0;
        dialogueText.text = "";

        GameManager.Instance.SetDialogueActive(true);
        ShowLine(dialogueLines[index]);
    }

    private DialogueLineData[] ParseDialogueLines(string[] rawLines)
    {
        var parsedLines = new System.Collections.Generic.List<DialogueLineData>();

        foreach (string rawLine in rawLines)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
                continue;

            DialogueLineData lineData = ParseLine(rawLine);
            parsedLines.Add(lineData);
        }

        return parsedLines.ToArray();
    }

    private DialogueLineData ParseLine(string rawLine)
    {
        string[] parts = rawLine.Split('|', 2);

        if (parts.Length < 2)
        {
            Debug.LogWarning($"Line format invalid: '{rawLine}'. Using Narrator by default.");
            return new DialogueLineData(CharacterID.Narrator, rawLine);
        }

        string characterText = parts[0].Trim();
        string dialogueText = parts[1].Trim();

        if (!System.Enum.TryParse(characterText, true, out CharacterID character))
        {
            Debug.LogWarning($"Unknown character '{characterText}' in line: '{rawLine}'. Using Narrator.");
            character = CharacterID.Narrator;
        }

        return new DialogueLineData(character, dialogueText);
    }

    private void SetCharacterActive(CharacterID character)
    {
        Narrator.SetActive(false);
        MainCharacter.SetActive(false);
        Friend.SetActive(false);
        Enemy1.SetActive(false);
        Enemy2.SetActive(false);

        switch (character)
        {
            case CharacterID.Narrator:
                Narrator.SetActive(true);
                break;

            case CharacterID.MainCharacter:
                MainCharacter.SetActive(true);
                break;

            case CharacterID.Friend:
                Friend.SetActive(true);
                break;

            case CharacterID.Enemy1:
                Enemy1.SetActive(true);
                break;

            case CharacterID.Enemy2:
                Enemy2.SetActive(true);
                break;
        }
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

    private void ShowLine(DialogueLineData lineData)
    {
        SetCharacterActive(lineData.character);
        currentLine = lineData.text;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(lineData.text));
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

[System.Serializable]
public struct DialogueLineData
{
    public CharacterID character;
    public string text;

    public DialogueLineData(CharacterID character, string text)
    {
        this.character = character;
        this.text = text;
    }
}