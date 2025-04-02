using System.Collections;
using UnityEngine;

public class door_script : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private BoxCollider boxColliderTrigger;
    [SerializeField] private BoxCollider doorCollider;
    private Vector3 openPosition;
    [SerializeField] private float doorSpeed = 1.0f;
    private bool isOpened = false;
    private Vector3 originalPosition;
    private Coroutine currentDoorRoutine;

    //value changed in the lockDoorScript
    private bool doorLocked = false;

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

    public void setDoorUnlocked()
    {
        doorLocked = false;
        doorCollider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOpened)
        {
            Debug.Log("Player entered door trigger");
            OpenDoor();
        }
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
            isOpened = true;
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
