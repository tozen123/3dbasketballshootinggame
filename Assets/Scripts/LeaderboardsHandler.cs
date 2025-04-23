using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardsHandler : MonoBehaviour
{
    [Header("UI References")]
    public GameObject scrollview_content;
    public GameObject historyPanelPrefab;

    void Start()
    {
        DisplayHistory();
        InitializeDummyData();
    }

    void InitializeDummyData()
    {
        PlayerPrefs.SetString("ScoreHistory_20231112_101500", "150|2023-11-12");
        PlayerPrefs.SetString("ScoreHistory_20231113_142300", "200|2023-11-13");
        PlayerPrefs.SetString("ScoreHistory_20231114_083010", "175|2023-11-14");
        PlayerPrefs.SetString("ScoreHistory_20231115_093210", "180|2023-11-15");
        PlayerPrefs.SetString("ScoreHistory_20231116_114500", "190|2023-11-16");
        PlayerPrefs.SetString("ScoreHistory_20231117_102230", "220|2023-11-17");
        PlayerPrefs.SetString("ScoreHistory_20231118_145600", "160|2023-11-18");
        PlayerPrefs.SetString("ScoreHistory_20231119_110300", "200|2023-11-19");
        PlayerPrefs.SetString("ScoreHistory_20231120_123400", "210|2023-11-20");
        PlayerPrefs.SetString("ScoreHistory_20231121_134500", "190|2023-11-21");
        PlayerPrefs.SetString("ScoreHistory_20231122_115600", "205|2023-11-22");
        PlayerPrefs.SetString("ScoreHistory_20231123_150700", "225|2023-11-23");
        PlayerPrefs.SetString("ScoreHistory_20231124_161800", "230|2023-11-24");
        PlayerPrefs.SetString("ScoreHistory_20231125_172900", "245|2023-11-25");
        PlayerPrefs.SetString("ScoreHistory_20231126_183000", "260|2023-11-26");
        PlayerPrefs.SetString("ScoreHistory_20231127_194100", "270|2023-11-27");
        PlayerPrefs.SetString("ScoreHistory_20231128_205200", "280|2023-11-28");
        PlayerPrefs.SetString("ScoreHistory_20231129_210300", "290|2023-11-29");
        PlayerPrefs.SetString("ScoreHistory_20231130_221400", "300|2023-11-30");
        PlayerPrefs.SetString("ScoreHistory_20231201_232500", "310|2023-12-01");
        PlayerPrefs.SetString("ScoreHistory_20231202_103600", "320|2023-12-02");
        PlayerPrefs.SetString("ScoreHistory_20231203_114700", "330|2023-12-03");
        PlayerPrefs.SetString("ScoreHistory_20231204_125800", "340|2023-12-04");
        PlayerPrefs.SetString("ScoreHistory_20231205_140900", "350|2021-12-05");

        // Update HistoryKeys to include all entries
        string allKeys =
            "ScoreHistory_20231112_101500;" +
            "ScoreHistory_20231113_142300;" +
            "ScoreHistory_20231114_083010;" +
            "ScoreHistory_20231115_093210;" +
            "ScoreHistory_20231116_114500;" +
            "ScoreHistory_20231117_102230;" +
            "ScoreHistory_20231118_145600;" +
            "ScoreHistory_20231119_110300;" +
            "ScoreHistory_20231120_123400;" +
            "ScoreHistory_20231121_134500;" +
            "ScoreHistory_20231122_115600;" +
            "ScoreHistory_20231123_150700;" +
            "ScoreHistory_20231124_161800;" +
            "ScoreHistory_20231125_172900;" +
            "ScoreHistory_20231126_183000;" +
            "ScoreHistory_20231127_194100;" +
            "ScoreHistory_20231128_205200;" +
            "ScoreHistory_20231129_210300;" +
            "ScoreHistory_20231130_221400;" +
            "ScoreHistory_20231201_232500;" +
            "ScoreHistory_20231202_103600;" +
            "ScoreHistory_20231203_114700;" +
            "ScoreHistory_20231204_125800;" +
            "ScoreHistory_20231205_140900;";

        PlayerPrefs.SetString("HistoryKeys", allKeys);
        PlayerPrefs.Save();
    }

    void DisplayHistory()
    {
        foreach (Transform child in scrollview_content.transform)
        {
            Destroy(child.gameObject);
        }

        string allKeys = PlayerPrefs.GetString("HistoryKeys", "");
        string[] keys = allKeys.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);

        List<(int score, string date)> sortedEntries = new List<(int score, string date)>();

        foreach (string key in keys)
        {
            string historyEntry = PlayerPrefs.GetString(key, "");
            if (!string.IsNullOrEmpty(historyEntry))
            {
                string[] parts = historyEntry.Split('|');
                int savedScore = int.Parse(parts[0]);
                string savedDate = parts[1];
                sortedEntries.Add((savedScore, savedDate));
            }
        }

        sortedEntries.Sort((entry1, entry2) => entry2.score.CompareTo(entry1.score));

        foreach (var entry in sortedEntries)
        {
            GameObject historyPanel = Instantiate(historyPanelPrefab, scrollview_content.transform);
            TextMeshProUGUI nameText = historyPanel.transform.GetChild(0).Find("Name").GetComponent<TextMeshProUGUI>();

            TextMeshProUGUI scoreText = historyPanel.transform.GetChild(0).transform.GetChild(1).Find("ScoreText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI dateText = historyPanel.transform.GetChild(0).Find("DateText").GetComponent<TextMeshProUGUI>();

            scoreText.text = entry.score.ToString();
            dateText.text = entry.date.ToString();
            nameText.text = PlayerPrefs.GetString("userName").ToString();
        }
    }

    public void SwitchScene(string scene_name)
    {
        LoadingScreenManager.Instance.LoadScene(scene_name);
    }
}
