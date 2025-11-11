using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
        HandleJump();
        HandleCrouch();
        ApplyGravity();
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");   // A/D, Left/Right
        float z = Input.GetAxis("Vertical");     // W/S, Forward/Back

        Vector3 move = transform.right * x + transform.forward * z;

        float currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;  // keeps player grounded

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleCrouch()
    {
        // Hold-to-crouch
        // isCrouching = Input.GetKey(KeyCode.LeftControl);

        // Toggle-crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isCrouching = !isCrouching;

        controller.height = Mathf.Lerp(
            controller.height,
            isCrouching ? crouchHeight : standingHeight,
            Time.deltaTime * 8f
        );
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
