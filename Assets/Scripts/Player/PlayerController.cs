using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static float distanceFromTarget;
    [SerializeField] float toTarget;

    [SerializeField]
    private Transform cameraOrientation;

    [SerializeField]
    LayerMask layersToHit;

    [SerializeField]
    private float clickRange = 100f;


    [Header("Speed Parameters")]
    [SerializeField]
    private float walkSpeed = 5f;

    [SerializeField]
    private float runSpeed = 8f;


    [Header("Events")]
    public GameEvent onMenu;


    private CapsuleCollider capsuleCollider;

    private Rigidbody rb;

    private float moveSpeed;

    private float movementX, movementZ;

    private Vector3 moveDirection;

    private Ray ray;

    private RaycastHit hitInfo;
    private RaycastHit[] hits;


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

    private bool IsCursorVisible = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        capsuleCollider = GetComponent<CapsuleCollider>();

        ray = new Ray(Vector3.zero, Vector3.forward);

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

    public void OnMenu(InputAction.CallbackContext context) // Make curser in/visible
    {
        if (IsCursorVisible)
        {
            moveSpeed = walkSpeed;
            onMenu.TriggerEvent();

            IsCursorVisible = false;
        }
        else if (!IsCursorVisible)
        {
            moveSpeed = 0;
            onMenu.TriggerEvent();

            IsCursorVisible = true;
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context) // Destroy wall player is looking at
    {
        if (context.started && !IsCursorVisible)
        {
            ray.origin = transform.position;
            ray.direction = cameraOrientation.forward;

            if (Physics.Raycast(ray, out hitInfo, clickRange, layersToHit))
            {
                GameObject mazeWall = hitInfo.transform.gameObject;
                mazeWall.GetComponent<MeshRenderer>().enabled = false;
                mazeWall.GetComponent<BoxCollider>().isTrigger = true;
            }
        }
    }

    public void OnRightClick(InputAction.CallbackContext context) // Build a wall where the player is looking
    {
        if (context.started && !IsCursorVisible)
        {
            ray.origin = transform.position;
            ray.direction = cameraOrientation.forward;

            if (Physics.Raycast(ray, out hitInfo, clickRange, layersToHit))
            {
                GameObject mazeWall = hitInfo.transform.gameObject;
                mazeWall.GetComponent<MeshRenderer>().enabled = true;
                mazeWall.GetComponent<BoxCollider>().isTrigger = false;
            }
        }
    }

    public void OnSwitchFlight(InputAction.CallbackContext context) // Dis/enable flight
    {
        rb.useGravity = !rb.useGravity;

        capsuleCollider.enabled = !capsuleCollider.enabled;
    }
}