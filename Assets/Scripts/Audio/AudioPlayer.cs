using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play(); // Play the music
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop(); // Stop the music
        }
    }

    public void PauseMusic()
    {
        if (audioSource != null)
        {
            audioSource.Pause(); // Pause the music
        }
    }

    public void ResumeMusic()
    {
        if (audioSource != null)
        {
            audioSource.UnPause(); // Resume the music
        }
    }
}
