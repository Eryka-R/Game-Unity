using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip checkpointSound;

    [Header("UI")]
    [SerializeField] private GameObject victoryScreen;

    [SerializeField] private GameObject gameOverScreen;

    [Header("Respawn Settings")]
    [SerializeField] private int MAX_DEATHS = 3;

    private UIManager uiManager;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private int deathCount = 0;
    

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn(){
        if (deathCount >= MAX_DEATHS){ 
            
            uiManager.GameOver();

            return;}

        deathCount++;
        print($"Player died. Death count: {deathCount}");
        SoundManager.instance.StopSound();

        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();

        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
