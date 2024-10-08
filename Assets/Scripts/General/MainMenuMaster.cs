using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMaster : MonoBehaviour
{
    public void SwitchScene(string scene_name)
    {
        LoadingScreenManager.Instance.LoadScene(scene_name);
    }

    public void Play(string scene_name)
    {
        ButtonSoundController.Instance.PlayButtonSound();
        PlayerPrefs.SetInt("Play_ScorePoints", 0);
        PlayerPrefs.Save();

        LoadingScreenManager.Instance.LoadScene(scene_name);
    }

    public void ExitGame()
    {
        ButtonSoundController.Instance.PlayButtonSound();

        WindowActionDialogSystem.Instance
            .SetTitle("Exit Game")
            .SetMessage("Are you sure you want to exit the game?")
            .OnYesClick(() =>
            {
                Application.Quit();  
            })
            .OnNoClick(() =>
            {

            })
            .Show();
    }
}
