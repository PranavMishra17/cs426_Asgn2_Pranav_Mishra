using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 3f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform playerCamera;
    public float cameraRotationSpeed = 2f;

    private float groundCheckRadius = 0.2f;
    private bool isGrounded;
    private CharacterController characterController;
    private float verticalVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Player movement
        MovePlayer();

        // Player look around
        LookAround();

        // Player jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = (transform.forward * vertical + transform.right * horizontal) * speed;
        characterController.Move(movement * Time.deltaTime);

        // Apply gravity
        if (!isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -0.5f; // Reset vertical velocity when grounded
        }

        // Apply vertical velocity to simulate jumping and falling
        movement.y = verticalVelocity;
        characterController.Move(movement * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Rotate the player's body based on mouse input
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera up and down based on mouse input
        float newRotationX = Mathf.Clamp(playerCamera.eulerAngles.x - mouseY, -90f, 90f);
        playerCamera.rotation = Quaternion.Euler(newRotationX, playerCamera.eulerAngles.y, 0f);
    }

    void Jump()
    {
        verticalVelocity = jumpForce;
    }
}
