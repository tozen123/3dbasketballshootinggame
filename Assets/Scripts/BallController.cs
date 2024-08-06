using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    public bool isStateFlying = false;
    private bool isCoroutineRunning = false;

    void Start()
    {
        trailRenderer.enabled = false;
    }

    void Update()
    {
        if (isStateFlying)
        {
            trailRenderer.enabled = true;
            if (isCoroutineRunning)
            {
                StopCoroutine(DisableTrailRendererWithDelay());
                isCoroutineRunning = false;
            }
        }
        else
        {
            if (!isCoroutineRunning)
            {
                StartCoroutine(DisableTrailRendererWithDelay());
            }
        }
    }

    private IEnumerator DisableTrailRendererWithDelay()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1.0f);
        trailRenderer.enabled = false;
        isCoroutineRunning = false;
    }
}
