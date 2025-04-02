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
            isOpened = false;
            originalPosition = door.transform.position;
            openPosition = originalPosition + Vector3.up * 5.0f;
            doorCollider.enabled = true;
            doorCollider.isTrigger = true;
        }
    }


    public void setDoorLocked()
    {
        doorLocked = true;
    }

    public void unlockDoor()
    {
        doorLocked = false;
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

    public void CloseDoor(bool forceClose)
    {
        if ((isOpened || forceClose) && door != null)
        {
            isOpened = false;
            Debug.Log("Door is now closed. isOpened set to false.");
            if (currentDoorRoutine != null)
            {
                StopCoroutine(currentDoorRoutine);
            }
            currentDoorRoutine = StartCoroutine(HandleDoorLogic(door.transform.position, originalPosition));
        }
        else
        {
            Debug.Log("CloseDoor() not executed. Door is already closed or another issue occurred.");
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
