using UnityEngine;

public class LogicManagerScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject endTeleporterObject;
    [SerializeField] GameObject startTeleporterObject;

    private BoxCollider endTeleporterCollider;
    private BoxCollider startTeleporterCollider;
    private CapsuleCollider playerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endTeleporterCollider = endTeleporterObject.GetComponent<BoxCollider>();
        startTeleporterCollider = startTeleporterObject.GetComponent<BoxCollider>();
        playerCollider = player.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollider.bounds.Intersects(endTeleporterCollider.bounds))
        {
            player.transform.position = startTeleporterObject.transform.position;
        }
    }
}
