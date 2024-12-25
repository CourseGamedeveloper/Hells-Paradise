using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()//this for button to play the game
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void About()//this for button for explain the game
    {
        SceneManager.LoadScene("About");
    }
    public void BACK()//this for button in About to return to the Menu
    {
        SceneManager.LoadScene("Menu");
    }
}
