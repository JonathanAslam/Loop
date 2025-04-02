using System;
using UnityEngine;

public class LogicManagerScript : MonoBehaviour
{
    // The following variables are used to manage the teleportation of the player
    [SerializeField] GameObject player;
    [SerializeField] GameObject endTeleporterObject;
    [SerializeField] GameObject startTeleporterObject;

    private BoxCollider endTeleporterCollider;
    private BoxCollider startTeleporterCollider;
    private CharacterController playerController;

    // The following variables are used to manage unlocking and locking the doors between rounds
    [SerializeField] private BoxCollider entryCollider;
    [SerializeField] private BoxCollider exitCollider;

    void StartNewRound()
    {
        // Logic to start a new round
        // Things needed:
        // Unlock entry door
        entryCollider.GetComponent<door_script>().setDoorUnlocked();
        // Update the room items (position, type, color, etc)

        // lock exit door
        exitCollider.GetComponent<exit_room_script>().CloseDoor(true);
        exitCollider.GetComponent<exit_room_script>().setDoorLocked();
        // UI things (increase round number, etc)
        // other things needed for future...
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endTeleporterCollider = endTeleporterObject.GetComponent<BoxCollider>();
        startTeleporterCollider = startTeleporterObject.GetComponent<BoxCollider>();
        playerController = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController == null || endTeleporterCollider == null || startTeleporterCollider == null)
        {
            return; // Exit if any required component is missing
        }

        // Calculate the bounds of the CharacterController manually
        Bounds playerBounds = new Bounds(
            playerController.transform.position + playerController.center,
            new Vector3(playerController.radius * 2, playerController.height, playerController.radius * 2)
        );
        if (playerBounds.Intersects(endTeleporterCollider.bounds))
        {
            // Teleport the player to the start teleporter position. Temporarily disable the character controller to allow the position change without the move method
            // playerController.Move() normally works fine, but since the door is locked, we need to disable the controller to avoid collding with the entry door
            playerController.enabled = false;
            player.transform.position = startTeleporterObject.transform.position;
            playerController.enabled = true;
            // function to start next round
            StartNewRound();
        }
    }
}
