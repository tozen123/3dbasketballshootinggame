using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    private Vector3 _offset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.35f;
    [SerializeField] private float SwitchingSmoothSpeed = 0.65f;
    private Vector3 _currentVelocity = Vector3.zero;

    private float originalSmoothSpeed;
    private void Start()
    {
        Application.targetFrameRate = 60;
        originalSmoothSpeed = smoothSpeed;

    }

    // set camera offset by getting the position 
    private void Awake() => _offset = transform.position - target.position;


    // smooth follow the target
    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothSpeed);
    }
    public void SetTarget(Transform newTarget)
    {
        smoothSpeed = SwitchingSmoothSpeed;


        target = newTarget;
        StartCoroutine(ResetSmoothSpeed());

        //_offset = transform.position - target.position;
    }
    private IEnumerator ResetSmoothSpeed()
    {
        yield return new WaitForSeconds(0.5f); 

        smoothSpeed = originalSmoothSpeed;
    }

}
