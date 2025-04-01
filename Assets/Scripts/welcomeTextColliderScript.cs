using Unity.VisualScripting;
using UnityEngine;

public class welcomeTextColliderScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private CapsuleCollider playerBoxCollider;
    [SerializeField] private GameObject colliderGameObject;
    private BoxCollider welcomeColliderBoxCollider; 
    [SerializeField] private GameObject welcomeTextObject;

    private bool isFirstRound = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBoxCollider = player.GetComponent<CapsuleCollider>();
        welcomeColliderBoxCollider = colliderGameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the players box collider is touching the welcome text box collider, then the welcome text will appear
        if (playerBoxCollider.bounds.Intersects(welcomeColliderBoxCollider.bounds) && isFirstRound)
        {
            //only want show this message before the player enters the room for the first time
            isFirstRound = false;
            welcomeTextObject.SetActive(true);
            //disable the players movement while the welcome text is displayed
            player.GetComponent<PlayerControllerScript>().enabled = false;
        }
        //if the welcomeTextObject is showing (the canvas) and the player presses the return key, then the welcome text will disappear
        if (welcomeTextObject.activeSelf == true && Input.GetKeyDown(KeyCode.Return))
        {
            welcomeTextObject.SetActive(false);
            player.GetComponent<PlayerControllerScript>().enabled = true;
        }
    }
}
