using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main menu UI, including starting, quitting, and navigating between scenes.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Loads the game scene.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QUIT()
    {
        Application.Quit();
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Restarts the current game scene by loading the next scene in the build index.
    /// </summary>
    public void RestGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
