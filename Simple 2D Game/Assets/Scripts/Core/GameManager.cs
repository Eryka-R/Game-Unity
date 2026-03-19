using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool dialogueActive { get; private set; }
    public bool pauseActive { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueUI;

    [Header("Coins UI")]
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Level 1")]
    [SerializeField] private GameObject platform1;

    private int coins = 0;

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

        if (platform1 != null)
        {
            platform1.SetActive(false);
        }
    }

    public void AddCoins(int amount){
        coins += amount;
        // Debug.Log($"Coins: {coins}");
        coinsText.text = $"x {coins}";
    }

    public int GetCoins(){
        return coins;
    }



    #region Dialogue Management

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

    public bool IsInputBlocked()
    {
        return dialogueActive || pauseActive;
    }
    #endregion


    #region Levels Management
    public void appearObbject(TextoID id)
    {
        switch (id)
        {
            case TextoID.TercerDialogo:
                ActivateLevel1();
                break;
            default:
                Debug.LogWarning($"No action defined for {id}");
                break;
        }
    }
    #endregion


    #region Level 1
    public void ActivateLevel1()
    {
        if (platform1 != null)
        {
            platform1.SetActive(true);
        }
    }
    #endregion
}