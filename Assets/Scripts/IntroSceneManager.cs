using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// This class handles the video playback for the intro scene.
public class IntroSceneManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The VideoPlayer component responsible for playing the intro video.")]
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_LoopPointReached;
    }

    // Triggered when the video finishes playing.
    private void VideoPlayer_LoopPointReached(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene("Menu");
    }
}
