using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] Transform cameraHolder;
    [SerializeField] Camera playerCam;

    //jumping
    private float verticalVelocity = 0.0f;
    private float gravity = -15.0f; //9.81f
    private bool isGrounded = true;

    // movement (horizontal and vertical)
    public float movementSpeed;
    private CharacterController characterController;


    //looking around
    public float mouseSensHorizontal = 2.0f;
    public float mouseSensVertical = 2.0f;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;


    private void HandlePlayerLook()
    {

        //player looking around
        rotationX += Input.GetAxisRaw("Mouse X") * mouseSensHorizontal;
        rotationY += Input.GetAxisRaw("Mouse Y") * mouseSensVertical;

        //clamp the user from looking vertically past +/- 90 degrees
        rotationY = Mathf.Clamp(rotationY, -90.0f, 90.0f);

        //horizontal mouse movement affects the Y-axis camera movement, while vertical mouse movement affects the X-axis movement
        //this is why they are swapped
        cameraHolder.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

    }

    private void HandlePlayerMovement()
    {
        // Check if the player is grounded
        isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f; // Reset vertical velocity when grounded
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Handle jump input
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = Mathf.Sqrt(2.0f * -gravity * 2.0f); // Calculate jump velocity for 2 units height
            Debug.Log("Player is jumping");
        }

        // Get horizontal and forward movement input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveForward = Input.GetAxisRaw("Vertical");

        // Move in the direction of the camera, constrained to the XZ plane
        Vector3 cameraForward = playerCam.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraHorizontal = playerCam.transform.right;
        cameraHorizontal.y = 0;
        cameraHorizontal.Normalize();

        Vector3 playerMovementDirection = (cameraForward * moveForward + cameraHorizontal * moveHorizontal).normalized;

        // Combine horizontal and vertical movement
        Vector3 movement = playerMovementDirection * movementSpeed + Vector3.up * verticalVelocity;

        // Move the character
        characterController.Move(movement * Time.deltaTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //check if player wants to sprint or jump. if so, do it
        //check sprint first
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 20.0f;
        }
        else
        {
            movementSpeed = 10.0f;
        }

        HandlePlayerLook();
        HandlePlayerMovement(); //handles player movement (x, y, z)
    }
}

