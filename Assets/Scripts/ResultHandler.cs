using System;
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

            SaveScoreHistory(score);
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

    private void SaveScoreHistory(int score)
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        string historyKey = "ScoreHistory_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

        string historyEntry = score.ToString() + "|" + currentDate;

        PlayerPrefs.SetString(historyKey, historyEntry);

        string allKeys = PlayerPrefs.GetString("HistoryKeys", "");
        allKeys += historyKey + ";";
        PlayerPrefs.SetString("HistoryKeys", allKeys);

        PlayerPrefs.Save();
    }
}
