using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WindowDialogSystem : MonoBehaviour
{
    public static WindowDialogSystem Instance { get; private set; }

    public GameObject popUpWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;
    public Button okButton;

    private Action onOkClicked;

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

    public WindowDialogSystem SetTitle(string title)
    {
        titleText.text = title;
        return this;
    }

    public WindowDialogSystem SetMessage(string message)
    {
        messageText.text = message;
        return this;
    }

    public WindowDialogSystem OnClick(Action onOkClick)
    {
        onOkClicked = onOkClick;
        return this;
    }

    public void Show()
    {
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(OnOkButtonClicked);
        popUpWindow.SetActive(true);
    }

    private void OnOkButtonClicked()
    {
        ButtonSoundController.Instance.PlayButtonSound();

        onOkClicked?.Invoke();
        popUpWindow.SetActive(false);
    }

    public void Hide()
    {
        popUpWindow.SetActive(false);
    }
}
