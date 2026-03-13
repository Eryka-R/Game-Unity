using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;


    private void Awake() {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
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

        Time.timeScale = _status ? 0 : 1;
        print("Game " + (_status ? "Paused" : "Resumed"));
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
}

