using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WindowActionDialogSystem : MonoBehaviour
{
    public static WindowActionDialogSystem Instance { get; private set; }

    public GameObject popUpWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button yesButton;
    public Button noButton;
    public Button closeButton;

    private Action onYesClicked;
    private Action onNoClicked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public WindowActionDialogSystem SetTitle(string title)
    {
        titleText.text = title;
        return this;
    }

    public WindowActionDialogSystem SetMessage(string message)
    {
        messageText.text = message;
        return this;
    }

    public WindowActionDialogSystem OnYesClick(Action onYesClick)
    {
        onYesClicked = onYesClick;
        return this;
    }

    public WindowActionDialogSystem OnNoClick(Action onNoClick)
    {
        onNoClicked = onNoClick;
        return this;
    }

    public WindowActionDialogSystem SetYesButtonText(string text)
    {
        yesButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
        return this;
    }

    public WindowActionDialogSystem SetNoButtonText(string text)
    {
        noButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
        return this;
    }

    public void Show()
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked); // Add the close button action

        popUpWindow.SetActive(true);
    }

    private void OnYesButtonClicked()
    {
        ButtonSoundController.Instance.PlayButtonSound();
        onYesClicked?.Invoke();
        popUpWindow.SetActive(false);
    }

    private void OnNoButtonClicked()
    {
        ButtonSoundController.Instance.PlayButtonSound();
        onNoClicked?.Invoke();
        popUpWindow.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        ButtonSoundController.Instance.PlayButtonSound();
        Hide();
    }

    public void Hide()
    {
        popUpWindow.SetActive(false);
    }

    public void ShowWearOrBuyDialog(string itemType, string color, int cost, Action onWear, Action onBuy)
    {
        SetTitle(itemType + " Options")
            .SetMessage("Do you want to wear or buy the " + color + " " + itemType.ToLower() + "? It costs " + cost + " points.")
            .SetYesButtonText("Wear")
            .SetNoButtonText("Buy Only")
            .OnYesClick(onWear)
            .OnNoClick(onBuy)
            .Show();
    }
}
