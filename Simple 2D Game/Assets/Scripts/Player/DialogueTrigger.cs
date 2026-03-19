using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextoID dialogueID;
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private Dialogue dialogueSystem;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        
        if (triggerOnce && hasTriggered)
            return;

        if (GameManager.Instance == null || GameManager.Instance.dialogueActive)
            return;

        if (dialogueSystem == null)
        {
            Debug.LogError("Dialogue system is not assigned in DialogueTrigger.");
            return;
        }

        dialogueSystem.StartDialogue(dialogueID);
        hasTriggered = true;

        GameManager.Instance.appearObbject(dialogueID);
    }
}