using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {

        [SerializeField] float chaseDistance = 5f;

        Health health;
        Fighter fighter;
        GameObject player;
        Mover mover;

        Vector2 origLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceRepose = 0f;

        float waypointDwellTime = 1.5f;
        int currentWaypointIndex = 0;


        [SerializeField] float suspicionLength = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        [SerializeField] float minRepose = 5f;
        [SerializeField] float maxRepose = 8f;

        float reposeSet = 0f;



        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();

            origLocation = transform.position;

            reposeSet = UnityEngine.Random.Range(minRepose, maxRepose);
        }

        public void Update()
        {

            if (health.IsDead()) return;

            if (fighter != null && InChaseRange(player) && fighter.CanAttack(player))
            {
                
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionLength)
            {
                SuspicionBehavior();
            }
            else if (timeSinceRepose > reposeSet)
            {
                TwitchBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();

        }

        private void TwitchBehavior()
        {
            mover.ReposeIdle();
            timeSinceRepose = 0f;
            reposeSet = UnityEngine.Random.Range(minRepose, maxRepose);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("trigger!" + other.gameObject.name);
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceRepose += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = origLocation;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMovement(nextPosition, false);
                
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPointLocation(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < waypointTolerance;
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private bool InChaseRange(GameObject player)
        {
            Vector2 distance = player.transform.position - this.transform.position;
            
            return distance.magnitude <= chaseDistance;
        }


        //Called by Unity
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);

        }

    }
}