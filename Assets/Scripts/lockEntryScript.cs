using UnityEngine;

public class lockDoorScript : MonoBehaviour
{
    //object for the box collider
    [SerializeField] private BoxCollider roomEntryBoxCollider;
    [SerializeField] GameObject player;
    private door_script door_script;

    void Awake() {
        door_script = FindFirstObjectByType<door_script>();

    }

    void Start()
    {
        roomEntryBoxCollider = GetComponent<BoxCollider>();
        if (roomEntryBoxCollider == null)
        {
            Debug.Log("Box Collider is not assigned in the inspector");
        }
        
        if (player == null) {
            Debug.Log("Player GameObject is not assigned in the inspector");
        }

    }

    void Update()
    {
        if (roomEntryBoxCollider.bounds.Contains(player.transform.position))
        {
            Debug.Log("Player is inside the room");
            if (door_script != null)
            {
                door_script.setDoorLocked();
                Debug.Log("Door is locked");
            }
        }
    }
}
