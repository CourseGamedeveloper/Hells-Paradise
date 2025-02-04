using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages game states, including opening gates, handling victory, and game over events.
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject Gate;
    public GameObject TextGateOpen;

    /// <summary>
    /// Opens the gate and displays the corresponding UI text.
    /// </summary>
    public void OpenGate()
    {
        if (Gate != null)
        {
            TextGateOpen.SetActive(true);
            StartCoroutine(HideTextAfterDelay(3f));
            Debug.Log("Gate opened");

            // Move the gate upwards
            Vector3 currentPosition = Gate.transform.position;
            Gate.transform.position = new Vector3(currentPosition.x, currentPosition.y + 5f, currentPosition.z);
        }
    }

    /// <summary>
    /// Hides the gate open text after a delay.
    /// </summary>
    /// <param name="delay">The delay in seconds before hiding the text.</param>
    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TextGateOpen.SetActive(false);
    }

    /// <summary>
    /// Loads the victory scene when the player wins the game.
    /// </summary>
    public void WinGame()
    {
        SceneManager.LoadScene("WinGame");
    }

    /// <summary>
    /// Loads the game over scene when the player loses.
    /// </summary>
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
