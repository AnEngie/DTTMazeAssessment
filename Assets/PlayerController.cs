using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float moveSpeed;
    public float movementX;
    public float movementY;
    public float movementZ;

    public Transform orientation;

    Vector3 moveDirection;

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
        }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
        }
    }

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        moveSpeed = walkSpeed;
    }

    private void FixedUpdate()
    {
        moveDirection = orientation.forward * movementZ + orientation.right * movementX;

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementX = context.ReadValue<Vector2>().x;
        movementZ = context.ReadValue<Vector2>().y;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
            moveSpeed = runSpeed;
        }
        else if (context.canceled)
        {
            IsRunning = false;
            moveSpeed = walkSpeed;
        }
    }
}