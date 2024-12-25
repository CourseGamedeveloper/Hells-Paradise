using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
//this class for the video
public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointRached;
    }
    private void VideoPlayer_loopPointRached(VideoPlayer videoPlayer)//after the video finish go to the menu
    {
        SceneManager.LoadScene("Menu");
    }
}
 