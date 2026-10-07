using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    private Vector3 closedPosition;
    private Vector3 openPosition;
    public float lip = 0.3f;

    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    [Header("Detection Settings")]
    public float detectionDistance = 0.2f;

    private bool isOpening = false;
    private bool isClosing = false;
    private BoxCollider doorCollider;

    public bool isBreakable = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = transform.position + 
            new Vector3(transform.localScale.x - lip, 0, 0) * direction.x +
            new Vector3(0, transform.localScale.y - lip, 0) * direction.y;

        doorCollider = GetComponent<BoxCollider>();
        if (doorCollider == null)
        {
            Debug.LogError("Door3D_Vector requires a BoxCollider on the same GameObject.");
        }
    }

    void Update()
    {
        // Handle door movement
        if (isOpening)
        {
            MoveDoor(openPosition);
        }
        else if (isClosing)
        {
            //print(IsObstacleInWay());

            // Only close if no obstacle
            if (!IsObstacleInWay())
            {
                MoveDoor(closedPosition);
            }
            

            //MoveDoor(closedPosition);
            //IsObstacleInWay();
        }
    }

    public void ToggleDoor()
    {
        if (isBreakable) {
            BreakDoor(); return;
        }

        if (!isOpening && !isClosing)
        {
            // Decide direction based on current position
            float distanceToClosed = Vector3.Distance(transform.position, closedPosition);
            float distanceToOpen = Vector3.Distance(transform.position, openPosition);

            if (distanceToClosed < distanceToOpen)
                isOpening = true;
            else
                isClosing = true;
        }
    }

    private void BreakDoor()
    {
        transform.gameObject.SetActive(false);
    }

    private void MoveDoor(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            isOpening = false;
            isClosing = false;
        }
    }

    private bool IsObstacleInWay()
    {
        // Cast a box toward the closed position

        Vector3 direction = (closedPosition - transform.position).normalized;
        float distance = detectionDistance;

        Vector3 halfExtents = doorCollider.bounds.extents;
        RaycastHit hit;

        return Physics.BoxCast(transform.position, halfExtents, direction, out hit, transform.rotation, distance, 0)
               && hit.collider.attachedRigidbody != null;

        /*if (Physics.BoxCast(transform.position, halfExtents, direction, out hit, transform.rotation, distance, 0)) {
            hit.collider.GetComponent<Rigidbody>().position = hit.collider.GetComponent<Rigidbody>().position - new Vector3(0.5f, 0, 0); }

        return false;*/
    }
}