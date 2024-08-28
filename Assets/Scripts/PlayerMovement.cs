using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 _playerInput;

    [Header("Attributes")]
    [SerializeField] public float movementSpeed = 10f;
    [SerializeField] private float turningSpeed = 360f;

    [Header("Joystick")]
    [SerializeField] private FixedJoystick movementJoystick;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Misc")]
    public bool canMove;
    public bool haveBall;
    public Animator animator;
    private void Start()
    {
        canMove = true;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (haveBall)
        {
            animator.SetBool("withBall", true);
        }
        else 
        {
            animator.SetBool("withBall", false);
        }
        GetPlayerInput();
        Look();
    }


    // get the joystick input
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
        if (canMove)
        {
            Move();
        }
    }

    // rotate the player base on the input
    private void Look()
    {
        if (_playerInput != Vector3.zero)
        {
            var relative = (transform.position + _playerInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turningSpeed * Time.deltaTime);
        }
    }

    // move the player using the charactercontroller 
    private void Move()
    {
        Vector3 movement = _playerInput.normalized * movementSpeed * Time.deltaTime;
        _characterController.Move(movement);

        bool isMoving = movement.magnitude > 0;

        // animations
        if (isMoving)
        {
            if (haveBall)
            {
                animator.SetBool("isRunningWithBall", true);
                animator.SetBool("isRunning", false); 
            }
            else
            {
                animator.SetBool("isRunningWithBall", false); 
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            animator.SetBool("isRunningWithBall", false);
            animator.SetBool("isRunning", false);
        }

    }

    
}
