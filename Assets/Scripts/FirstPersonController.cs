using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public float lookSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
        HandleJump();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * lookSensitivity;
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x);
    }

    void HandleJump()
    {
        if (jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}