using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool dialogueActive { get; private set; }
    public bool pauseActive { get; private set; }

    [Header("Player State")]
    [SerializeField] private int playerHealth = 3;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }

    public void SetPlayerHealth(int value)
    {
        playerHealth = Mathf.Max(0, value);
    }

    public void DamagePlayer(int damage)
    {
        playerHealth = Mathf.Max(0, playerHealth - damage);
    }

    public void HealPlayer(int amount)
    {
        playerHealth += amount;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void SetDialogueActive(bool active)
    {
        dialogueActive = active;

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(active);
        }

        RefreshTimeScale();
    }

    public void SetPauseActive(bool active)
    {
        pauseActive = active;
        RefreshTimeScale();
    }

    private void RefreshTimeScale()
    {
        Time.timeScale = (dialogueActive || pauseActive) ? 0f : 1f;
    }
}