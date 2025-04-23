using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Loader : MonoBehaviour
{
    public float loadingDuration = 8f; // Duration of the fake loading in seconds
    private float timer = 0f;

    public Image loading_fg; // The foreground image with a fill effect
    public TextMeshProUGUI loadingCountLabel; // The percentage label

    void Start()
    {
        if (loading_fg != null)
        {
            loading_fg.fillAmount = 0f; // Initialize the fill amount
        }

        if (loadingCountLabel != null)
        {
            loadingCountLabel.text = "0%"; // Initialize the label
        }

        Debug.Log("Loading started...");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (loading_fg != null)
        {
            // Update the fill amount based on the elapsed time
            loading_fg.fillAmount = Mathf.Clamp01(timer / loadingDuration);
        }

        if (loadingCountLabel != null)
        {
            // Calculate and display the loading percentage
            int progressPercentage = Mathf.RoundToInt((timer / loadingDuration) * 100);
            loadingCountLabel.text = progressPercentage + "%";
        }

        if (timer >= loadingDuration)
        {
            CheckPlayerPrefs();
        }
    }

    void CheckPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("userName"))
        {
            Debug.Log("UserName exists. Transitioning to MainMenu...");
            SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the exact name of your scene
        }
        else
        {
            Debug.Log("UserName not found. Transitioning to UserInputWelcome...");
            SceneManager.LoadScene("UserInputWelcome"); // Replace "UserInputWelcome" with the exact name of your scene
        }
    }
}
