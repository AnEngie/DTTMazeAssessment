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

    [SerializeField]
    private PlayerLook playerLook;

    [SerializeField]
    LayerMask mazeWalls;

    public Rigidbody rb;

    private float moveSpeed;

    private float movementX, movementZ;

    Vector3 moveDirection;

    Ray ray;
    RaycastHit hit;

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

    private bool _IsCursorVisible = false;

    private bool IsCursorVisible
    {
        get
        {
            return _IsCursorVisible;
        }
        set
        {
            _IsCursorVisible = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        ray = new Ray(Vector3.zero, Vector3.forward);

        moveSpeed = walkSpeed;
    }

    void Start()
    {
        mazeWalls = LayerMask.GetMask("Maze Walls");
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
        if (context.started && !IsCursorVisible)
        {
            moveSpeed = runSpeed;
        }
        else if (context.canceled && !IsCursorVisible)
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

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (IsCursorVisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerLook.enabled = true;
            moveSpeed = walkSpeed;

            IsCursorVisible = false;
        }
        else if (!IsCursorVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerLook.enabled = false;
            moveSpeed = 0;

            IsCursorVisible = true;
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started && !IsCursorVisible)
        {
            ray.origin = transform.position;
            ray.direction = cameraOrientation.forward;

            if (Physics.Raycast(ray, out hit, 1000f, mazeWalls))
            {
                Debug.Log("HIT!" + hit.transform.gameObject.name);
                hit.transform.gameObject.SetActive(false);
            }

            if (Physics.Raycast(ray, out hit, 1000f, mazeWalls))
            {
                Debug.Log("HIT!" + hit.transform.gameObject.name);
                hit.transform.gameObject.SetActive(false);
            }
        }
    }
}