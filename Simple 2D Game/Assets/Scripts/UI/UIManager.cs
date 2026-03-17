using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class UIManager : MonoBehaviour
{

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    private bool instructionsCompleted = false;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;

    [Header("Settings Menu")]
    [SerializeField] private GameObject settingsMenuScreen;

    [Header("Instructions")]
    [SerializeField] private GameObject instructionsScreen;
    [SerializeField] private GameObject instructionsAviso;
    [SerializeField] private GameObject[] instructionsScreens;
    [SerializeField] private GameObject[] buttonNext;
    private int currentInstruction = 0;

    private void Awake() {
        instructionsCompleted = PlayerPrefs.GetInt("InstructionsCompleted", 0) == 1;
        if (gameOverScreen != null && pauseScreen == null){
           gameOverScreen.SetActive(false);
            pauseScreen.SetActive(false);
        }
        if (mainMenuScreen != null){
            mainMenuScreen.SetActive(true);
        }
        if (settingsMenuScreen != null){
            settingsMenuScreen.SetActive(false);
        }
        if (instructionsScreen != null){
            instructionsScreen.SetActive(false);
        }
        if (instructionsAviso != null){
            instructionsAviso.SetActive(false);
        }
        if (instructionsScreens != null){
            foreach (GameObject screen in instructionsScreens){
                screen.SetActive(false);
            }
        }
        if (buttonNext != null){
            foreach (GameObject button in buttonNext){
                button.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isActive = pauseScreen.activeSelf;
            PauseGame(!isActive);
        }
    }

    #region Game Over
    public void GameOver(){
        gameOverScreen.SetActive(true);
        SoundManager.instance.StopSound();
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool _status)
    {
        pauseScreen.SetActive(_status);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPauseActive(_status);
        }
        // Time.timeScale = _status ? 0 : 1;
        // print("Game " + (_status ? "Paused" : "Resumed"));
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.1f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.1f);
    }
    #endregion


    #region Main Menu
    public void playGame()
    {
        if (!instructionsCompleted)
        {
            if (instructionsAviso != null)
            {
                UITextFileLoader.ChangeTextFromFile(instructionsAviso, TextoID.AvisoInstruccion);
                StartCoroutine(MostrarAviso());
            }
            return;
        }
        SceneManager.LoadScene(1);
    }

    IEnumerator MostrarAviso()
    {
        instructionsAviso.SetActive(true);

        yield return new WaitForSeconds(10f);

        instructionsAviso.SetActive(false);

        openInstructions();
    }

    public void openSettings()
    {
        mainMenuScreen.SetActive(false);
        settingsMenuScreen.SetActive(true);
    }

    public void openInstructions()
    {
        mainMenuScreen.SetActive(false);
        instructionsScreen.SetActive(true);
        currentInstruction = 0;
        buttonNext[currentInstruction].SetActive(true);
        buttonNext[1].SetActive(false);
        ShowInstruction(currentInstruction);
    }

    #endregion

    #region Settings Menu
    public void MainMenuFromSettings()
    {
        mainMenuScreen.SetActive(true);
        settingsMenuScreen.SetActive(false);
    }

    #endregion

    #region Instructions

    public void NextInstructions()
    {
        instructionsScreens[currentInstruction].SetActive(false);

        currentInstruction++;

        if (currentInstruction == instructionsScreens.Length - 1){
            buttonNext[0].SetActive(false);
            buttonNext[1].SetActive(true);
        }

        if (currentInstruction >= instructionsScreens.Length)
        {
            instructionsScreen.SetActive(false);
            foreach (GameObject screen in instructionsScreens){
                screen.SetActive(false);
            }
            mainMenuScreen.SetActive(true);
            currentInstruction = 0;
            instructionsCompleted = true;
            PlayerPrefs.SetInt("InstructionsCompleted", 1);
            PlayerPrefs.Save();
            return;
        }

        ShowInstruction(currentInstruction);
    }

    private void ShowInstruction(int index)
    {
        instructionsScreens[index].SetActive(true);
    }

    public void MainMenuFromInstructions()
    {
        instructionsScreen.SetActive(false);
        foreach (GameObject screen in instructionsScreens){
            screen.SetActive(false);
        }
        mainMenuScreen.SetActive(true);
    }

    # endregion
}

