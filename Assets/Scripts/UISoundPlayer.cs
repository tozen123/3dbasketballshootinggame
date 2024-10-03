using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundPlayer : MonoBehaviour
{
    public void PlayUISound()
    {
        ButtonSoundController.Instance.PlayButtonSound();
    }
}
