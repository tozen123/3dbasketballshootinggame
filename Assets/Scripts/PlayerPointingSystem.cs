using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPointingSystem : MonoBehaviour
{
    // Singleton, so that this class can be accessed everywhere 
    public static PlayerPointingSystem Instance { get; private set; }

    [Header("Attributes")]
    public int ShootPoints = 0;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI pointTextIndicator;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float maxScale = 1.5f;

    private Vector3 originalScale;

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
        originalScale = pointTextIndicator.rectTransform.localScale;
    }

    private void Update()
    {
        if (!pointTextIndicator)
        {
            return;
        }
        if (ShootPoints.ToString().Equals("0"))
        {
            pointTextIndicator.text = "O";

        } else
        {
            pointTextIndicator.text = ShootPoints.ToString();

        }
    }

    public void AddPoint(int toAddPoints)
    {
        ShootPoints += toAddPoints;
        StartCoroutine(AnimateText());
    }

    public void ResetPoint()
    {
        ShootPoints = 0;
    }

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
}
