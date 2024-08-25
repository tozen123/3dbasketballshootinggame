using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float staticDuration = 2.0f;

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

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {

        yield return StartCoroutine(FadeIn());

        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingText.text = "Loading... " + (progress * 100f).ToString("F0") + "%";
            yield return null;
        }

        yield return new WaitForSeconds(staticDuration);
        yield return StartCoroutine(FadeOut());

        loadingScreen.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        float startTime = Time.time;
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float startTime = Time.time;
        float alpha = 1f;

        while (alpha > 0f)
        {
            alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
