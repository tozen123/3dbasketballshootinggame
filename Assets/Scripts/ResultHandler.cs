using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject PanelWin;
    [SerializeField] private GameObject PanelLose;

    [SerializeField] private TextMeshProUGUI Gained;

    [Header("Score")]
    public int score;
    public int targetScore;

    void Start()
    {
        score = PlayerPrefs.GetInt("Play_ScorePoints");

      

        if (score >= targetScore)
        {
            PanelWin.SetActive(true);
            PanelLose.SetActive(false);

            Gained.text = score.ToString().Replace("0", "O");
        }
        else
        {
            PanelWin.SetActive(false);
            PanelLose.SetActive(true);
        }
    }
    public void Continue(bool isLose)
    {
        ButtonSoundController.Instance.PlayButtonSound();
        if (!isLose)
        {
            int currentPlayerPoints = PlayerPrefs.GetInt("Player_Points", 0); 

            int updatedPoints = currentPlayerPoints + score;

            PlayerPrefs.SetInt("Play_ScorePoints", 0);
            PlayerPrefs.SetInt("Player_Points", updatedPoints);
            PlayerPrefs.Save();
        }
        LoadingScreenManager.Instance.LoadScene("MainMenu");
    }

}
