using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundController : MonoBehaviour
{
    // Singleton instance
    public static ButtonSoundController Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSoundClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    public void PlayButtonSound()
    {
        if (buttonSoundClip != null)
        {
            audioSource.clip = buttonSoundClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip is null, cannot play sound.");
        }
    }

    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
