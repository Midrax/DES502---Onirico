﻿using UnityEngine;
using UnityEngine.AI;

public class SightController : MonoBehaviour
{

    [SerializeField] float fieldOfViewAngle = 110f;             // Number of degrees, centred on forward, for the enemy see.
    [SerializeField] float minimumDetectableSpeed = 1f;         // The enemy can hear players who have at least this movement speed.
    public bool playerInSight = false;                          // Whether or not the player is currently sighted.
    public Vector3 personalLastSighting = Vector3.zero;         // Last place this enemy spotted the player.

    //  References
    NavMeshAgent nav = null;                                    // Reference to the NavMeshAgent component.
    SphereCollider hearingRadius = null;                        // Reference to the sphere collider trigger component.
    CommonVariables commonVariables = null;                     // Reference to global variables.
    GameObject player = null;                                   // Reference to the player.

    Vector3 previousSighting = Vector3.zero;                    // Where the player was sighted last frame.                                      

    void Awake()
    {
        // Setting up the references.
        nav = GetComponent<NavMeshAgent>();
        hearingRadius = GetComponent<SphereCollider>();
        commonVariables = GameObject.FindObjectOfType<CommonVariables>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Set the personal sighting and the previous sighting to the reset position.
        personalLastSighting = commonVariables.resetPlayerPosition;
        previousSighting = commonVariables.resetPlayerPosition;
    }

    void Update()
    {
        // If the last global sighting of the player has changed...
        if (commonVariables.playerPosition != previousSighting)
        {
            // ... then update the personal sighting to be the same as the global sighting.
            personalLastSighting = commonVariables.playerPosition;
        }

        // Set the previous sighting to the be the sighting from this frame.
        previousSighting = commonVariables.playerPosition;
    }

    void OnTriggerStay(Collider other)
    {
        // If the player has entered the trigger sphere...
        if (other.gameObject == player)
        {
            // By default the player is not in sight.
            playerInSight = false;

            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            // If the angle between forward and where the player is, is less than half the angle of view...
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                // ... and if a raycast towards the player hits something...
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, hearingRadius.radius))
                {
                    // ... and if the raycast hits the player...
                    if (hit.collider.gameObject == player)
                    {
                        // ... the player is in sight.
                        playerInSight = true;

                        // Set the last global sighting is the players current position.
                        commonVariables.playerPosition = player.transform.position;
                    }
                }
            }

            if (commonVariables.moveSpeed > minimumDetectableSpeed)
            {
                // ... and if the player is within hearing range...
                if (CalculatePathLength(player.transform.position) <= hearingRadius.radius)
                {
                    // ... set the last personal sighting of the player to the player's current position.
                    personalLastSighting = player.transform.position;
                }
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the player leaves the trigger zone...
        if (other.gameObject == player)
        {
            // ... the player is not in sight.
            playerInSight = false;
        }
    }


    float CalculatePathLength(Vector3 targetPosition)
    {
        // Create a path and set it based on a target position.
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }

        // Create an array of points which is the length of the number of corners in the path + 2.
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        // The first point is the enemy's position.
        allWayPoints[0] = transform.position;

        // The last point is the target position.
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points inbetween are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}
