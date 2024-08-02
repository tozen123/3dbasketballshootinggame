using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 _playerInput;
    [SerializeField] public float movementSpeed = 10f;
    [SerializeField] private float turningSpeed = 360f;
    [SerializeField] private FixedJoystick movementJoystick;
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GetPlayerInput();
        Look();
    }

    private void GetPlayerInput()
    {
        Vector3 right = cameraTransform.right;
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();
        right.y = 0;
        right.Normalize();

        _playerInput = right * movementJoystick.Horizontal + forward * movementJoystick.Vertical;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Look()
    {
        if (_playerInput != Vector3.zero)
        {
            var relative = (transform.position + _playerInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turningSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        Vector3 movement = _playerInput.normalized * movementSpeed * Time.deltaTime;
        _characterController.Move(movement);
    }
}
