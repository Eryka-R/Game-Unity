using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool dialogueActive { get; private set; }
    public bool pauseActive { get; private set; }

    private bool dialoguePausesGame;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueUI;

    [Header("Coins UI")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private int MAX_COINS = 5;

    [Header("Room 1")]
    [SerializeField] private GameObject platform1;
    [SerializeField] private GameObject conversation1Character;
    [SerializeField] private GameObject conversation1Above;

    [Header("Room 4")]
    [SerializeField] private GameObject column1Room4;
    [SerializeField] private GameObject column01Room4;
    [SerializeField] private GameObject heartCollectible1;

    [Header("Room 5")]
    [SerializeField] private GameObject door1;
    [SerializeField] private GameObject door2;
    [SerializeField] private float timeBlockDoors = 10f;

    [Header("Room 6")]
    [SerializeField] private GameObject trap1Room6;
    [SerializeField] private GameObject trap2Room6;

    [Header("Room 9")]
    [SerializeField] private GameObject column1Room9;
    [SerializeField] private GameObject column2Room9;
    [SerializeField] private GameObject EnemyRoom9;
    [SerializeField] private float timeColumnsDoors = 20f;

    [Header("Room 12")]
    [SerializeField] private GameObject column1Room12;

    [Header("Room 13")]
    [SerializeField] private GameObject ConversationFriend;
    [SerializeField] private GameObject ConversationFriendNotCompleted;




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

        dialogueUI.SetActive(false);
        platform1.SetActive(false);
        column1Room4.SetActive(true);
        column01Room4.SetActive(false);
        heartCollectible1.SetActive(false);
        door1.SetActive(false);
        door2.SetActive(false);
        column1Room9.SetActive(false);
        column2Room9.SetActive(false);
        EnemyRoom9.SetActive(false);
        column1Room12.SetActive(true);
        ConversationFriend.SetActive(false);
        ConversationFriendNotCompleted.SetActive(false);
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

    public void SetDialogueActive(bool active, bool pausesGame = true)
    {
        dialogueActive = active;
        dialoguePausesGame = active && pausesGame;

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
        Time.timeScale = (pauseActive || dialoguePausesGame) ? 0f : 1f;
    }

    public bool IsInputBlocked()
    {
        return pauseActive || dialoguePausesGame;
    }
    #endregion


    #region Room Management
    public void appearObject(TextoID id)
    {
        switch (id)
        {
            case TextoID.TercerDialogo:
                ActivateRoom1();
                break;
            case TextoID.Conversacion1:
                if (conversation1Above != null)
                {
                    conversation1Above.SetActive(false);
                }
                break;
            case TextoID.Conversacion1Above:
                if (conversation1Character != null)
                {
                    conversation1Character.SetActive(false);
                }
                break;
            default:
                Debug.LogWarning($"No action defined for {id}");
                break;
        }
    }
    #endregion


    public void triggerObject(triggersID id)
    {
        Debug.Log($"Triggering object for trigger ID: {id}");
        switch (id)
        {
            case triggersID.None:
                break;
            case triggersID.InitTriggerRoom4:
                column01Room4.SetActive(true);
                break;
            case triggersID.TriggerRoom4:
                heartCollectible1.SetActive(true);
                column1Room4.SetActive(false);
                column01Room4.SetActive(false);
                break;
            case triggersID.TriggerRoom5InsideHouse:
                door1.SetActive(true);
                door2.SetActive(true);
                StartCoroutine(DeactivateDoorsAfterTime(timeBlockDoors));
                break;
            case triggersID.KillEnemyRoom6:
                trap2Room6.SetActive(false);
                trap1Room6.SetActive(false);
                break;
            case triggersID.TriggerRoom9:
                column1Room9.SetActive(true);
                column2Room9.SetActive(true);
                EnemyRoom9.SetActive(true);
                StartCoroutine(DeactivateColumnsAfterTime(timeColumnsDoors));
                break;
            case triggersID.TriggerRoom12:
                column1Room12.SetActive(false);
                break;
            case triggersID.TriggerEndFriend:
                if (coins == MAX_COINS){
                    ConversationFriend.SetActive(true);
                }
                else{
                    ConversationFriendNotCompleted.SetActive(true);
                }
                break;

            case triggersID.Victory:
                if (UIManager.Instance != null){
                    UIManager.Instance.Victory();
                }
                break;
            default:
                Debug.LogWarning($"No action defined for trigger {id}");
                break;
        }
    }

    IEnumerator DeactivateDoorsAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        door1.SetActive(false);
        door2.SetActive(false);
    }

    IEnumerator DeactivateColumnsAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        column1Room9.SetActive(false);
        column2Room9.SetActive(false);
        EnemyRoom9.SetActive(false);
    }


    #region Room 1
    public void ActivateRoom1()
    {
        if (platform1 != null)
        {
            platform1.SetActive(true);
        }
    }
    #endregion
}