using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPointingSystem : MonoBehaviour
{
    // Singleton, so that this class can be accessed everywhere 
    public static PlayerPointingSystem Instance { get; private set; }

    [Header("Attributes")]
    public int ShootPoints = 0;
    public int StreakCount = 0;
    public int StreakThreshold = 3;
    public int StreakBonusPoints = 25;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI pointTextIndicator;
    [SerializeField] private RectTransform shotButton;
    [SerializeField] private GameObject textOnFire;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float maxScale = 1.5f;

    [Header("Effects Attributes")]
    [SerializeField] private float wiggleMagnitude;
    [SerializeField] private float wiggleSpeed;

    [Header("Mode")]

    [SerializeField] private bool isArcade = false;

    private Vector3 originalScale;
    private Coroutine wiggleCoroutine;

    // Set as Singleton Class
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

    private void Start()
    {
        if (!pointTextIndicator)
        {
            return;
        }

        // Load the saved score from PlayerPrefs
        ShootPoints = PlayerPrefs.GetInt("Play_ScorePoints", 0);
        originalScale = pointTextIndicator.rectTransform.localScale;
        UpdatePointText();
    }

    private void Update()
    {
        if (!pointTextIndicator)
        {
            return;
        }

        // If streak is achieve add effects
        if (StreakCount >= StreakThreshold)
        {
            if (wiggleCoroutine == null)
            {
                wiggleCoroutine = StartCoroutine(WiggleButton());
            }

            textOnFire.SetActive(true);
        }
        else
        {
            textOnFire.SetActive(false);


            if (wiggleCoroutine != null)
            {
                StopCoroutine(wiggleCoroutine);
                wiggleCoroutine = null;
                shotButton.transform.rotation = Quaternion.identity;
            }
        }
    }

    // get point method
    public int GetPoint()
    {
        return ShootPoints;
    }

    // add point method
    public void AddPoint(int toAddPoints)
    {
        if (!isArcade)
        {
            if (StreakCount >= StreakThreshold)
            {
                toAddPoints += StreakBonusPoints;
            }

            ShootPoints += toAddPoints;
            StreakCount++;

            PlayerPrefs.SetInt("Play_ScorePoints", ShootPoints);
            PlayerPrefs.Save();

            
        }
        else
        {
            ShootPoints += toAddPoints;
        }

        StartCoroutine(AnimateText());
        UpdatePointText();
    }

    // reset to zero points, for arcade missing shots
    public void ResetPoint()
    {
        ShootPoints = 0;

        PlayerPrefs.SetInt("Play_ScorePoints", ShootPoints);
        PlayerPrefs.Save();

        UpdatePointText();
    }

    // reset streak when missed
    public void ResetStreak()
    {
        StreakCount = 0;
    }

    private void UpdatePointText()
    {
        if (pointTextIndicator)
        {
            string pointsText = ShootPoints.ToString().Replace("0", "O");
            pointTextIndicator.text = pointsText;
        }
    }

    // animation
    private IEnumerator AnimateText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration / 2)
        {
            float scale = Mathf.Lerp(originalScale.x, maxScale, (elapsedTime / (animationDuration / 2)));
            pointTextIndicator.rectTransform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < animationDuration / 2)
        {
            float scale = Mathf.Lerp(maxScale, originalScale.x, (elapsedTime / (animationDuration / 2)));
            pointTextIndicator.rectTransform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pointTextIndicator.rectTransform.localScale = originalScale;
    }

    private IEnumerator WiggleButton()
    {
        while (true)
        {
            float angle = Mathf.Sin(Time.time * Mathf.PI * wiggleSpeed) * wiggleMagnitude;
            shotButton.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
    }
}
