using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] float patrolSpeed = 2f;                // The nav mesh agent's speed when patrolling.
    [SerializeField] float chaseSpeed = 5f;                 // The nav mesh agent's speed when chasing.
    [SerializeField] float chaseWaitTime = 5f;              // The amount of time to wait when the last sighting is reached.
    [SerializeField] float patrolWaitTime = 1f;             // The amount of time to wait when the patrol way point is reached.
    [SerializeField] Transform[] patrolWayPoints = null;    // An array of transforms for the patrol route.


    EnemyViewController EnemyViewController = null;                           // Reference to the EnemyViewController script.
    NavMeshAgent nav = null;                                // Reference to the nav mesh agent.
    Transform player = null;                                // Reference to the player's transform.
    GlobalVariables GlobalVariables = null;           // Reference to the last global sighting of the player.

    float chaseTimer = 0f;                                  // A timer for the chaseWaitTime.
    float patrolTimer = 0f;                                 // A timer for the patrolWaitTime.

    int wayPointIndex = 0;                                  // A counter for the way point array.

    void Awake()
    {
        // Setting up the references.
        EnemyViewController = GetComponent<EnemyViewController>();
        nav = GetComponent<NavMeshAgent>();
        nav.baseOffset = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GlobalVariables = GameObject.FindGameObjectWithTag("GameController").GetComponent<GlobalVariables>();
    }

    void Update()
    {
        // If the player is in sight and is alive...
        if (EnemyViewController.playerInSight)
        {
            // ... shoot.
            Attacking();
        }

        // If the player has been sighted and isn't dead...
        else if (EnemyViewController.personalLastSighting != GlobalVariables.resetPlayerPosition)
        {
            // ... chase.
            Chasing();
        }

        // Otherwise...
        else
        {
            // ... patrol.
            Patrolling();
        }
    }

    void Attacking()
    {
        // Stop the enemy where it is.
        //nav.isStopped = true;
    }

    void Chasing()
    {
        // Create a vector from the enemy to the last sighting of the player.
        Vector3 sightingDeltaPos = EnemyViewController.personalLastSighting - transform.position;

        // If the the last personal sighting of the player is not close...
        if (sightingDeltaPos.sqrMagnitude > 4f)
        {
            // ... set the destination for the NavMeshAgent to the last personal sighting of the player.
            nav.destination = EnemyViewController.personalLastSighting;
        }

        // Set the appropriate speed for the NavMeshAgent.
        nav.speed = chaseSpeed;

        // If near the last personal sighting...
        if (nav.remainingDistance < nav.stoppingDistance)
        {
            // ... increment the timer.
            chaseTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if (chaseTimer >= chaseWaitTime)
            {
                // ... reset last global sighting, the last personal sighting and the timer.
                GlobalVariables.playerPosition = GlobalVariables.resetPlayerPosition;
                EnemyViewController.personalLastSighting = GlobalVariables.resetPlayerPosition;
                chaseTimer = 0f;
            }
        }
        else
        {
            // If not near the last sighting personal sighting of the player, reset the timer.
            chaseTimer = 0f;
        }
    }

    void Patrolling()
    {
        // Set an appropriate speed for the NavMeshAgent.
        nav.speed = patrolSpeed;

        // If near the next waypoint or there is no destination...
        if (nav.destination == GlobalVariables.resetPlayerPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            // ... increment the timer.
            patrolTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if (patrolTimer >= patrolWaitTime)
            {
                // ... increment the wayPointIndex.
                if (wayPointIndex == patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }

                // Reset the timer.
                patrolTimer = 0;
            }
        }
        else
        {
            // If not near a destination, reset the timer.
            patrolTimer = 0;
        }

        // Set the destination to the patrolWayPoint.
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
}
