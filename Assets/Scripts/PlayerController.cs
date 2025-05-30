using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private Camera playerCamera;
    private float xRotation = 0f;

    private bool isCursorLocked = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        LockCursor(true);
    }

    void Update()
    {
        if (isCursorLocked)
        {
            MovePlayer();
            JumpHandler();
            LookAround();
        }

        // ESC per sbloccare il cursore
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(false);
        }

        // Clic sinistro per ribloccare il cursore
        if (!isCursorLocked && Input.GetMouseButtonDown(0))
        {
            LockCursor(true);
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move = cameraRight * moveX + cameraForward * moveZ;

        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;

        float maxFallSpeed = -9.81f;
        if (velocity.y < maxFallSpeed)
        {
            velocity.y = maxFallSpeed;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void JumpHandler()
    {
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void LockCursor(bool shouldLock)
    {
        isCursorLocked = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
