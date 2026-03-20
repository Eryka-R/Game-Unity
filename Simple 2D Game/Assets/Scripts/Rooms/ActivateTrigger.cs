using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    [SerializeField] private triggersID triggerID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (GameManager.Instance == null)
            return;

        GameManager.Instance.triggerObject(triggerID);
    }
}