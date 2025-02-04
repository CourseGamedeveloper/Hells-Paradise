using UnityEngine;

/// <summary>
/// Manages audio playback based on the player's distance.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the player object.")]
    private Transform Player;

    private AudioSource _AudioSource;

    [SerializeField]
    [Tooltip("Maximum distance at which the audio is audible.")]
    private float Maxdistance;

    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Play_Audio();
    }

    /// <summary>
    /// Plays or stops audio based on the player's distance.
    /// </summary>
    private void Play_Audio()
    {
        if (_AudioSource != null && Player != null)
        {
            float distance = Vector3.Distance(transform.position, Player.position);

            if (distance <= Maxdistance)
            {
                if (!_AudioSource.isPlaying)
                {
                    _AudioSource.Play();
                }
            }
            else
            {
                if (_AudioSource.isPlaying)
                {
                    _AudioSource.Stop();
                }
            }
        }
    }
}
