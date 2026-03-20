using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    [SerializeField] private triggersID triggerID;
    [SerializeField] private bool triggerOnce = true;
    
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (GameManager.Instance == null)
            return;

        if (triggerOnce && hasBeenTriggered)
            return;

        GameManager.Instance.triggerObject(triggerID);
        hasBeenTriggered = true; 
    }
}