using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   
   public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void ABOUT()
    {
        SceneManager.LoadScene("About");

    }
    public void BACK_TOMENU()
    {
        SceneManager.LoadScene("Menu");
    }
}
