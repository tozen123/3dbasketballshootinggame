using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputWelcome : MonoBehaviour
{
    public TMP_InputField name_input; // Input field for user name
    public Button continueButton; // Continue button to save and proceed

    void Start()
    {
        // Add listener for the continue button click
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    void OnContinueClicked()
    {
        string userName = name_input.text.Trim();

        if (string.IsNullOrEmpty(userName))
        {
            // Show a dialog if the input is empty
            WindowDialogSystem.Instance
                .SetTitle("Invalid Input")
                .SetMessage("Please enter your name to proceed.")
                .OnClick(() =>
                {
                    Debug.Log("OK clicked on dialog.");
                })
                .Show();
        }
        else
        {
            // Save the user name in PlayerPrefs
            PlayerPrefs.SetString("userName", userName);
            PlayerPrefs.Save();

            Debug.Log("UserName saved: " + userName);

            // Transition to the next scene
            SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your target scene name
        }
    }
}
