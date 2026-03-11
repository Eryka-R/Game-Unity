using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;

    [Header("Audio")]
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip victorySound;

    private void Awake() {
        gameOverScreen.SetActive(false);
        // victoryScreen.SetActive(false);
    }


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
        UnityEditor.EditorApplication.isPlaying = false;
    }


}
