using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; }                // the character we are controlling

        CommonVariables commonVariables = null;
        [SerializeField] float patrolSpeed = 2f;                // The nav mesh agent's speed when patrolling.
        [SerializeField] float chaseSpeed = 5f;                 // The nav mesh agent's speed when chasing.
        [SerializeField] float chaseWaitTime = 5f;              // The amount of time to wait when the last sighting is reached.
        [SerializeField] float patrolWaitTime = 1f;             // The amount of time to wait when the patrol way point is reached.
        float chaseTimer = 0f;                                  // A timer for the chaseWaitTime.
        float patrolTimer = 0f;                                 // A timer for the patrolWaitTime.
        int wayPointIndex = 0;                                  // A counter for the way point array.
        [SerializeField] Transform[] patrolWayPoints = null;
        SightController sightController = null;                                    // Reference to the SightController script.

        private void Start()
        {
            commonVariables = GameObject.FindGameObjectWithTag("GameController").GetComponent<CommonVariables>();
            sightController = GetComponent<SightController>();
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }

        void Update()
        {
            // If the player is in sight and is alive...
            /*
            if (sightController.playerInSight)
            {
                // ... shoot.
                Attacking();
            }
            */

            // If the player has been sighted and isn't dead...
            // else if
            if (sightController.personalLastSighting != commonVariables.resetPlayerPosition)
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
            Vector3 sightingDeltaPos = sightController.personalLastSighting - transform.position;

            // If the the last personal sighting of the player is not close...
            if (sightingDeltaPos.sqrMagnitude > 4f)
            {
                // ... set the destination for the NavMeshAgent to the last personal sighting of the player.
                agent.SetDestination(sightController.personalLastSighting);
                agent.speed = chaseSpeed;
                character.Move(agent.desiredVelocity, false, false);
            }

            // Set the appropriate speed for the NavMeshAgent.

            // If near the last personal sighting...
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                character.Move(Vector3.zero, false, false);
                // ... increment the timer.
                chaseTimer += Time.deltaTime;

                // If the timer exceeds the wait time...
                if (chaseTimer >= chaseWaitTime)
                {
                    // ... reset last global sighting, the last personal sighting and the timer.
                    commonVariables.playerPosition = commonVariables.resetPlayerPosition;
                    sightController.personalLastSighting = commonVariables.resetPlayerPosition;
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
            agent.speed = patrolSpeed;

            // If near the next waypoint or there is no destination...
            if (agent.destination == commonVariables.resetPlayerPosition || agent.remainingDistance < agent.stoppingDistance)
            {
                character.Move(Vector3.zero, false, false);
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
            agent.SetDestination(patrolWayPoints[wayPointIndex].position);
            character.Move(agent.desiredVelocity, false, false);
        }
    }
}
