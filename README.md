# Welcome to Hell's Paradise Game
Embark on a perilous journey in Hell’s Paradise: Shadows of the Cursed Isle, a 2D action-adventure game where survival is the ultimate test. Explore a dark, mystical island filled with mythical creatures, ancient ruins, and deadly traps. As a lone warrior, you must uncover the island’s secrets, fight supernatural enemies, and manage scarce resources to survive the island’s challenges. Only the brave can escape the island’s curse!"

---

### opening process:
* Video when start the game
 ```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// This class handles the video playback for the intro scene.
public class IntroSceneManager : MonoBehaviour
{
    [SerializeField]
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
  
* basic Menu
* button About to read more information in the game .

---
* For more information You can read my wiki https://github.com/CourseGamedeveloper/Hells-Paradise/wiki/Hell's-Paradise-Game
## To play the first level:
link to the game: https://ibrahem-hurani.itch.io/hells-paradise
## Create By: Ibrahem Hurani
