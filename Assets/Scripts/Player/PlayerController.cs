using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5f;

    [SerializeField]
    private float runSpeed = 8f;

    [SerializeField]
    private Transform cameraOrientation;

    Rigidbody rb;

    private float moveSpeed;

    private float movementX, movementZ;

    Vector3 moveDirection;

    private bool _isJumping = false;

    private bool IsJumping
    {
        get
        {
            return _isJumping;
        }
        set
        {
            _isJumping = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        moveSpeed = walkSpeed;
    }

    private void FixedUpdate()
    {
        // Move based on direction player is looking
        moveDirection = cameraOrientation.forward * movementZ + cameraOrientation.right * movementX;

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

        // Movement on the Y axis
        if (IsJumping)
        {
            rb.AddForce(transform.up * moveSpeed, ForceMode.Force);
        }
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
            moveSpeed = runSpeed;
        }
        else if (context.canceled)
        {
            moveSpeed = walkSpeed;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset Y velocity before moving up
        }
        else if (context.canceled)
        {
            IsJumping = false;
        }
    }
}