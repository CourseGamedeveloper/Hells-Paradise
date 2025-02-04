using UnityEngine;
using TMPro;
using System.Collections;

public class VictoryMessage : MonoBehaviour
{
    public TextMeshProUGUI victoryText;
    public AudioClip victorySound;
    private AudioSource audioSource;

    private void Start()
    {
      
        victoryText.alpha = 0; // Start invisible
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(ShowVictoryMessage());
    }

    private IEnumerator ShowVictoryMessage()
    {
        // Play sound effect
        if (victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }

        // Fade in text
        for (float alpha = 0; alpha <= 1; alpha += Time.deltaTime)
        {
            victoryText.alpha = alpha;
            yield return null;
        }

        // Scale "VICTORY!" part for emphasis
        Vector3 originalScale = victoryText.transform.localScale;
        Vector3 targetScale = originalScale * 0.8f;
        float duration = 0.5f;

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            victoryText.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        // Restore scale
        victoryText.transform.localScale = originalScale;
    }
}
