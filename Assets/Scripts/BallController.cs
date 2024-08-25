using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Rigidbody rigidbody;


    [Header("Attributes")]
    public bool OnPlayer;
    public SphereCollider mainCollider;
    public bool isStateFlying = false;


    // Privates
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
            mainCollider.enabled = true;

            rigidbody.isKinematic = true;
        }
        else
        {
            rigidbody.isKinematic = false;

            if (!isCoroutineRunning)
            {
                StartCoroutine(DisableTrailRendererWithDelay());
            }
        }

        if(OnPlayer)
        {
            mainCollider.enabled = false;
        }
        else
        {
            mainCollider.enabled = true;
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
