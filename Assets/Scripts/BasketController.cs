using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{


    [Header("Camera")]
    public CameraController cameraController;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] basketSounds;

    [Header("Animator")]
    [SerializeField] private Animator animatorController;

    [Header("Mode Controller")]
    [SerializeField] private bool isArcade = false;
    [SerializeField] private bool isPlay = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            PlayRandomSound();

            cameraController.start = true;
    
            animatorController.SetTrigger("Shot");

            if (isArcade)
            {
                PlayerPointingSystem.Instance.AddPoint(1);
            }

            if (isPlay)
            {
                PlayerPointingSystem.Instance.AddPoint(10);

            }
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
