using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] private Vector3 _switchingOffset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.35f;
    [SerializeField] private float SwitchingSmoothSpeed = 0.65f;
    private Vector3 _currentVelocity = Vector3.zero;

    private float originalSmoothSpeed;
    private Vector3 originalOffset;

    private void Start()
    {
        Application.targetFrameRate = 60;
        originalSmoothSpeed = smoothSpeed;
        originalOffset = _offset;
    }

    private void Awake()
    {
        _offset = transform.position - target.position;
        originalOffset = _offset;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothSpeed);
    }

    public void SetTarget(Transform newTarget)
    {
        smoothSpeed = SwitchingSmoothSpeed;

        target = newTarget;
        _offset = _switchingOffset;

        StartCoroutine(ResetSmoothSpeedAndOffset());
    }

    private IEnumerator ResetSmoothSpeedAndOffset()
    {
        yield return new WaitForSeconds(0.5f); 

        smoothSpeed = originalSmoothSpeed;

        float elapsedTime = 0f;
        float duration = 0.5f; 
        Vector3 currentOffset = _offset;

        while (elapsedTime < duration)
        {
            _offset = Vector3.Lerp(currentOffset, originalOffset, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _offset = originalOffset;
    }
}
