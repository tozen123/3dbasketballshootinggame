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
        PlayerPrefs.SetInt("Play_ScorePoints", 0);
        PlayerPrefs.Save();

        LoadingScreenManager.Instance.LoadScene(scene_name);
    }
}
