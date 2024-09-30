using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (!volumeSlider)
        {
            volumeSlider = GameObject.FindGameObjectWithTag("AudioSlider").GetComponent<Slider>();

        }
        float currentVolume;
        audioMixer.GetFloat("MasterVolume", out currentVolume);
        volumeSlider.value = Mathf.Pow(10, currentVolume / 20);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        float volumeInDecibels = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MasterVolume", volumeInDecibels);
    }
}
