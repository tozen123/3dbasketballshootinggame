using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayGameMasterController : MonoBehaviour
{
    [Header("Game Settings")]

    [SerializeField] private string gameTitle = "Level ";
    [SerializeField] [TextArea] private string gameMessage = "Level ";

    [SerializeField] private string nextLevel = "--";

    [SerializeField] private float gameDuration = 60f; 
    [SerializeField] private float gameTargetPoints; 

    private float timeRemaining;
    private bool isGameRunning;


    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;
    void Start()
    {
        
        WindowDialogSystem.Instance
            .SetTitle(gameTitle)
            .SetMessage(gameMessage)
            .OnClick(() => StartGame())
            .Show();
    }

    void StartGame()
    {
        timeRemaining = gameDuration;
        isGameRunning = true;
        UpdateTimerDisplay();
    }
    void Update()
    {
        if (isGameRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isGameRunning = false;
                Timeout();
            }

            UpdateTimerDisplay();
        }
    }

    void Timeout()
    {
        if(PlayerPointingSystem.Instance.GetPoint() >= gameTargetPoints)
        {
            if(SceneManager.GetActiveScene().name == "PlayLevel5")
            {
                WindowDialogSystem.Instance
                  .SetTitle("Game")
                  .SetMessage("You have finished the Play Mission. Lets go to results")
                  .OnClick(() => LoadingScreenManager.Instance.LoadScene("PlayGameResult"))
                  .Show();
            }
            else
            {
                WindowDialogSystem.Instance
               .SetTitle("Game")
               .SetMessage("Good Job! You reached the target points for this level. Lets go to the next level")
               .OnClick(() => LoadingScreenManager.Instance.LoadScene(nextLevel))
               .Show();
            }
            
        }
        else
        {
            WindowDialogSystem.Instance
          .SetTitle("Game")
          .SetMessage("You failed!")
           .OnClick(() => LoadingScreenManager.Instance.LoadScene("PlayGameResult"))

          .Show();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60F);
        int seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);

        //timerText.text = string.Format("{0}:{1:00}", minutes, seconds).ToString().Replace("0", "O");
        timerText.text = string.Format("{0}", seconds).ToString().Replace("0", "O");

    }
}
