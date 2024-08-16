using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public float dashSpeed = 10f; // Speed during the dash
    public float dashDuration = 0.2f; // Duration of the dash
    public float dashCooldown = 1f; // Cooldown time between dashes
    public float doubleTapTime = 0.3f; // Time window to recognize double-tap

    private float verticalRotation = 0f;
    private Rigidbody rb;
    private Camera playerCamera;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -Mathf.Infinity;
    private float lastTapTimeA = -Mathf.Infinity;
    private float lastTapTimeD = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Handle double-tap dash input
        HandleDashInput();

        // Jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            DashMovement();
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
            dashTime = Time.time + dashDuration;
            lastDashTime = Time.time;
            rb.velocity = Vector3.zero; // Optional: Reset velocity to avoid conflicting with dash
            rb.AddForce(direction * dashSpeed, ForceMode.VelocityChange);
        }
    }

    void DashMovement()
    {
        if (Time.time >= dashTime)
        {
            isDashing = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
