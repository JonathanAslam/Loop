using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] Transform cameraHolder;
    [SerializeField] Camera playerCam;

    // movement
    public float movementSpeed = 15.0f;


    //looking around
    public float mouseSensHorizontal = 2.0f;
    public float mouseSensVertical = 2.0f;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveForward = Input.GetAxis("Vertical");

        //move in direction of camera, but according to the body in a flat plane movement capability, we dont want them to be able to fly off
        Vector3 cameraForward = playerCam.transform.forward;
        cameraForward.y = 0; // ignore the y axis
        cameraForward.Normalize();

        Vector3 cameraHorizontal = playerCam.transform.right;
        cameraHorizontal.y = 0;
        cameraHorizontal.Normalize();

        Vector3 playerMovementDirection = (cameraForward * moveForward + cameraHorizontal * moveHorizontal).normalized;
        transform.position += playerMovementDirection * movementSpeed * Time.deltaTime;


        //
        //FIXME : ISSUE WHEN RUNNING INTO WALL, PLAYER WILL START TO DRIFT WITHOUT ANY SOLUTION. FIX IN NEXT SESSION
        //

        // -------------------------------------------------
        //player looking around
        rotationX += Input.GetAxisRaw("Mouse X") * mouseSensHorizontal;
        rotationY += Input.GetAxisRaw("Mouse Y") * mouseSensVertical;

        //clamp the user from looking vertically past +/- 90 degrees
        rotationY = Mathf.Clamp(rotationY, -90.0f, 90.0f);

        //horizontal mouse movement affects the Y-axis camera movement, while vertical mouse movement affects the X-axis movement
        //this is why they are swapped
        cameraHolder.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
        
    }
}
