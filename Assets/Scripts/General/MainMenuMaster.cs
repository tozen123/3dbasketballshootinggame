using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMaster : MonoBehaviour
{
    public void SwitchScene(string scene_name)
    {
        LoadingScreenManager.Instance.LoadScene(scene_name);
    }
}