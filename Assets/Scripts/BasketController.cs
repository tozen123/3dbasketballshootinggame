using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public CameraController cameraController;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] basketSounds; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            PlayRandomSound();

            cameraController.start = true;
            PlayerPointingSystem.Instance.AddPoint(1);
        }
    }

    private void PlayRandomSound()
    {
        if (basketSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, basketSounds.Length); 
            audioSource.clip = basketSounds[randomIndex];           
            audioSource.Play();                                      
        }
    }
}
