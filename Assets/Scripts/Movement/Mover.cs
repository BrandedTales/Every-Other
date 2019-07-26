using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using RPG.Core;
using System;


namespace RPG.Movement
{

    public class Mover : MonoBehaviour, IAction
    {
        public Transform targetPosition;
        public Path path;

        [SerializeField] float walkSpeed = 2;
        [SerializeField] float runSpeed = 4;
        [SerializeField] float nextWaypointDistance = 0;
        [SerializeField] float repathRate = 0.5f;

        private int currentWaypoint = 0;
        private float lastRepath = float.NegativeInfinity;
        private float speed;

        public bool reachedEndOfPath;
        private bool isMoving = false;
        private bool targetIndicator = false;

        //Cache Fields
        private Animator myAnimator;
        private Seeker seeker;


        public void Start()
        {
            seeker = GetComponent<Seeker>();
            myAnimator = GetComponent<Animator>();

            speed = walkSpeed;
        }


        public void Update()
        {
            Move();
            ShowTargetIndicator();
        }



        public void Cancel()
        {
            targetPosition.position = transform.position;
            myAnimator.SetBool("Moving", false);  
        }

        public void StartMovement(Vector2 destination, bool partOfCombat)
        {
            targetPosition.position = destination;
            targetIndicator = true;
            isMoving = true;

            if (!partOfCombat) GetComponent<ActionScheduler>().StartAction(this);
            
        }

        private void Move()
        {
            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;
                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            }

            if (path == null)
            {
                // We have no path to follow yet, so don't do anything
                //myAnimator.SetBool("Moving", false);
                return;
            }

            // Check in a loop if we are close enough to the current waypoint to switch to the next one.
            // We do this in a loop because many waypoints might be close to each other and we may reach
            // several of them in the same frame.
            reachedEndOfPath = false;
            // The distance to the next waypoint in the path
            float distanceToWaypoint;
            distanceToWaypoint = WPMoveDistance();

            // Slow down smoothly upon approaching the end of the path
            // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            // Multiply the direction by our desired speed to get a velocity

            Vector3 velocity = dir * speed * speedFactor;
            //if (gameObject.tag == "Player") Debug.Log(velocity.magnitude + " vs " + nextWaypointDistance);
            if (velocity.magnitude > nextWaypointDistance)
            {
                myAnimator.SetBool("Moving", true);

            }
            AnimateMovement(velocity);
            transform.Translate(velocity * Time.deltaTime);
            //targetPosition.position -= velocity * Time.deltaTime;

        }

        private float WPMoveDistance()
        {
            float distanceToWaypoint;
            while (true)
            {
                // If you want maximum performance you can check the squared distance instead to get rid of a
                // square root calculation. But that is outside the scope of this tutorial.
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        reachedEndOfPath = true;
                        targetIndicator = false;
                        isMoving = false;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return distanceToWaypoint;
        }

        public void OnPathComplete(Path p)
        {
            //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

            // Path pooling. To avoid unnecessary allocations paths are reference counted.
            // Calling Claim will increase the reference count by 1 and Release will reduce
            // it by one, when it reaches zero the path will be pooled and then it may be used
            // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
            // take a path from the pool if possible. See also the documentation page about path pooling.
            p.Claim(this);
            if (!p.error)
            {
                if (path != null) path.Release(this);
                path = p;
                // Reset the waypoint counter so that we start to move towards the first point in the path
                currentWaypoint = 0;
            }
            else
            {
                p.Release(this);
            }
        }

        private void AnimateMovement(Vector3 velocity)
        {
            myAnimator.SetFloat("VelocityX", velocity.x);
            myAnimator.SetFloat("VelocityY", velocity.y);

            if (velocity.magnitude > nextWaypointDistance)
            {
                myAnimator.SetFloat("LookHorizontal", velocity.x);
                myAnimator.SetFloat("LookVertical", velocity.y);
            }
            else
            {
                myAnimator.SetBool("Moving", false);
            }
        }

        public void SetRun(bool isRunning)
        {
            if (isRunning)
            {
                speed = runSpeed;
                myAnimator.SetBool("Running", true);
            }
            else
            {
                speed = walkSpeed;
                myAnimator.SetBool("Running", false);
            }
        }


        public GameObject GetMoveTarget()
        {
            Transform[] ts = transform.GetComponentsInChildren<Transform>();

            foreach (Transform t in ts)
            {
                Debug.Log("Found the moveTarget for " + gameObject.name);
                if (t.gameObject.name == "MoveTarget") return t.gameObject;
            }

            return null;

        }

        private Animator GetAnimator()
        {
            //This is in a wrapper because I may have the animator buried in child game objects.
            return transform.GetComponent<Animator>();

        }

        public bool IsMoving() { return isMoving; }

        private void ShowTargetIndicator()
        {
            //if (gameObject.name == "Player") targetPosition.GetChild(0).gameObject.SetActive(targetIndicator);

        }

        //Called by Controller to cause character to twitch
        public void ReposeIdle()
        {
            if (!myAnimator.GetBool("Moving"))
                myAnimator.SetTrigger("Idle2");
        }


    }
}