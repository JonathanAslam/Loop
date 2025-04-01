using System.Collections;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class exit_room_script : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private BoxCollider boxColliderTrigger;
    [SerializeField] private BoxCollider doorCollider;
    private Vector3 openPosition;
    [SerializeField] private float doorSpeed = 1.0f;
    private bool isOpened = false;
    private Vector3 originalPosition;
    private Coroutine currentDoorRoutine;

    private bool doorLocked = true;

    void Awake()
    {
        boxColliderTrigger = GetComponent<BoxCollider>();
        if (door == null)
        {
            Debug.Log("Door GameObject is not assigned in the inspector");
        }
    }

    void Start()
    {
        if (door != null)
        {
            originalPosition = door.transform.position;
            openPosition = originalPosition + Vector3.up * 5.0f;
            doorCollider.enabled = true;
            doorCollider.isTrigger = true;
        }
    }

    public void setDoorLocked()
    {
        doorLocked = true;
        doorCollider.isTrigger = false;
    }

    public void unlockDoor() {
        doorLocked = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isOpened)
        {
            Debug.Log("Player exited door trigger");
            CloseDoor();
        }
    }
    public void OpenDoor()
    {
        if (!isOpened && door != null && !doorLocked)
        {
            if (currentDoorRoutine != null)
            {
                StopCoroutine(currentDoorRoutine);
            }
            currentDoorRoutine = StartCoroutine(HandleDoorLogic(door.transform.position, openPosition));
            // doorCollider.enabled = false;
            isOpened = true;
        }
        else
        {
            Debug.Log("Cant Open door");
            if (door == null)
            {
                Debug.Log("Door GameObject is not assigned in the inspector");
            }
            if (doorLocked)
            {
                Debug.Log("Door is locked");
            }
            if (isOpened)
            {
                Debug.Log("Door is already opened");
            }
        }
    }

    private void CloseDoor()
    {
        if (isOpened && door != null)
        {
            if (currentDoorRoutine != null)
            {
                StopCoroutine(currentDoorRoutine);
            }
            currentDoorRoutine = StartCoroutine(HandleDoorLogic(door.transform.position, originalPosition));
            isOpened = false;
        }
    }

    private IEnumerator HandleDoorLogic(Vector3 startPosition, Vector3 endPosition)
    {
        if (doorSpeed <= 0)
        {
            Debug.LogWarning("Door speed is set to 0 or less, setting it to 1");
            doorSpeed = 1.0f;
        }
        //door logic
        float timeElapsed = 0.0f;
        while (timeElapsed < doorSpeed)
        {
            door.transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / doorSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        door.transform.position = endPosition;
    }
}
