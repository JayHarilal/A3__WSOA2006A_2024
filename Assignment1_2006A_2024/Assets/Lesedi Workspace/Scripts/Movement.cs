using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public float dashSpeed = 10f;
    public float dashCooldown = 1f;
    public float doubleTapTime = 0.3f;
    public float verticalLookLimit = 90f;

    private float verticalRotation = 0f;
    private Rigidbody rb;
    public Transform playerCamera; 
    private bool isDashing = false;
    private Vector3 dashDirection;
    private float lastDashTime = -Mathf.Infinity;
    private float lastTapTimeA = -Mathf.Infinity;
    private float lastTapTimeD = -Mathf.Infinity;


    private PlayerInputManager playerInputManager;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private bool dashInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isDashing)
        {
            // Mouse look
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);

            // Handle vertical camera rotation
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            // transform.Rotate(Vector3.up * mouseX);

            // Handle dash input
            HandleDashInput();

            // Jump (if implemented)
             if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
            }
        }
    }


    void FixedUpdate()
    {
        if (isDashing)
        {
            // Move instantly during dash
            rb.velocity = dashDirection * dashSpeed;
            // Ensure the dash does not affect camera rotation
            Vector3 cameraRotation = playerCamera.localRotation.eulerAngles;
            playerCamera.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0f);

            Debug.Log("Player Rotation: " + transform.eulerAngles);
            Debug.Log("Camera Rotation: " + playerCamera.localRotation.eulerAngles);


            isDashing = false; // End the dash immediately after applying the force
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.fixedDeltaTime;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move);
    }

    void HandleDashInput()
    {
        if (Time.time >= lastDashTime + dashCooldown)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastTapTimeA <= doubleTapTime)
                {
                    StartDash(-transform.right); //StartDash(Vector3.left);Will dash to the left based on players direction
                }
                lastTapTimeA = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastTapTimeD <= doubleTapTime)
                {
                    StartDash(transform.right); //StartDash(Vector3.right);Will dash to the right based on players direction
                }
                lastTapTimeD = Time.time;
            }
        }
    }

    void StartDash(Vector3 direction)
    {
        if (!isDashing)
        {
            isDashing = true;
            dashDirection = direction.normalized;//Set the direction of the dash based on players direction
            lastDashTime = Time.time;
            // Lock the camera rotation
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, 0f);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
