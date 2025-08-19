using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    private float sensX;

    [SerializeField]
    private float sensY;


    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Stop player from turning their head upside down

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void OnMenu()
    {
        // Locking/Unlocking the cursor and enabling/disabling camera following mouse

        enabled = !enabled;

        if (enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        Cursor.visible = !Cursor.visible;

    }
}
