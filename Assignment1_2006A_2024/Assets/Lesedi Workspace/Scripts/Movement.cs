using UnityEngine;

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

            // Handle vertical camera rotation
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

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
                    StartDash(Vector3.left);
                }
                lastTapTimeA = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastTapTimeD <= doubleTapTime)
                {
                    StartDash(Vector3.right);
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
            dashDirection = direction;
            lastDashTime = Time.time;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
