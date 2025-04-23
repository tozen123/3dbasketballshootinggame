using System.Collections;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject Switch;
    [SerializeField] private GameObject Main;

    private CanvasGroup mainCanvasGroup;
    private CanvasGroup switchCanvasGroup;

    private bool isToggled = false;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private void Start()
    {
        mainCanvasGroup = Main.GetComponent<CanvasGroup>();
        switchCanvasGroup = Switch.GetComponent<CanvasGroup>();

        mainCanvasGroup.alpha = 1;
        switchCanvasGroup.alpha = 0;
        Switch.SetActive(false);
    }

    public void OpenAbout()
    {
        ButtonSoundController.Instance.PlayButtonSound();

        DialogManager.Instance.ShowDialog("Game Developed By: \n\nJiro Jaro \nGene Moises Narvas \nKenneth Baran \n\nMade With Unity");

    }

    // toggling method for ui switching 
    public void Toggle()
    {
        ButtonSoundController.Instance.PlayButtonSound();

        isToggled = !isToggled;

        if (isToggled)
        {
            StartCoroutine(FadeOut(mainCanvasGroup, Main));
            StartCoroutine(FadeIn(switchCanvasGroup, Switch));
        }
        else
        {
            StartCoroutine(FadeOut(switchCanvasGroup, Switch));
            StartCoroutine(FadeIn(mainCanvasGroup, Main));
        }
    }
    // fade animation 

    private IEnumerator FadeIn(CanvasGroup canvasGroup, GameObject target)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        target.SetActive(true);

    }

    // fade animation 
    private IEnumerator FadeOut(CanvasGroup canvasGroup, GameObject target)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f; 
        target.SetActive(false);
    }
}
