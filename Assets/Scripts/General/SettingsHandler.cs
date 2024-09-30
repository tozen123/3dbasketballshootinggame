using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject SettingsPanel;
    private bool isSettingsToggled = false;

    public void ToggleSettings()
    {
        ButtonSoundController.Instance.PlayButtonSound();
        isSettingsToggled = !isSettingsToggled;

        SettingsPanel.SetActive(isSettingsToggled);
    }

    public void SwitchScene(string scene_name)
    {
        SettingsPanel.SetActive(false);
        LoadingScreenManager.Instance.LoadScene(scene_name);
    }

}
