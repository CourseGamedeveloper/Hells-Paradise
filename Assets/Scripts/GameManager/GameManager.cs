using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Gate;
    public GameObject TextGateOpen;
    
    
    public void OpenGate()
    {
         if (Gate != null)
         {
            TextGateOpen.SetActive(true);
            StartCoroutine(HideTextAfterDelay(3f));
            Debug.Log("open");
                // Increment the Z position by 0.1
             Vector3 currentPosition = Gate.transform.position;
             Gate.transform.position = new Vector3(currentPosition.x, currentPosition.y + 5f, currentPosition.z);
         }
        
    }
    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TextGateOpen.SetActive(false); // כיבוי הטקסט
    }
    public void WinGame()
    {
        SceneManager.LoadScene("WinGame");
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}

