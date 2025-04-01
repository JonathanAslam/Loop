using UnityEngine;
using UnityEngine.UI;

public class playerInteractScript : MonoBehaviour
{
    //get access to camera for raytracing (raycast)
    [SerializeField] Camera cam;

    void Update()
    {
        //GetMouseButtonDown(0) is for the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shot ray");
            // think of a box in the first quadrant of a math grid (x,y)
            // top left point is 0,1
            // top right point is 1,1
            // bottom left point is 0,0
            // bottom right point is 1,0
            // middle of screen is 0.5, 0.5
            Ray shotRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(shotRay, out RaycastHit hit))
            {
                Debug.Log("Hit " + hit.collider.name);
                //if the object we hit is a button, invoke the onclick function of that button (assuming it has one)
                if (hit.collider.CompareTag("Exit") /* && room solved correctly*/)
                {
                    hit.collider.GetComponent<exit_room_script>().unlockDoor();
                    hit.collider.GetComponent<exit_room_script>().OpenDoor();
                    Debug.Log("Opening exit door");
                } else /* if room is not solved correctly */{
                    //end game
                }
            }
        }

    }


}
