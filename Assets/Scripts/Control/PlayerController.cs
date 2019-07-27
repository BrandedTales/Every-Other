using UnityEngine;
using System.Collections;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;
using PixelCrushers.DialogueSystem;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        //Cache fields
        CombatTarget target;
        Health health;
        Energy energy;
        Mover mover;

        [Header("Fatigue Values")]
        [SerializeField] float idleFatigue = 0.01f;
        [SerializeField] float walkFatigue = 0.5f;
        [SerializeField] float runFatigue = 1f;
        [SerializeField] float fatigue = 0f;

        [Header("Idle Fidgets")]
        [SerializeField] float minRepose = 5f;
        [SerializeField] float maxRepose = 8f;
        float reposeSet = 0f;
        float timeSinceRepose = 0f;

        // Use this for initialization
        void Start()
        {
            health = GetComponent<Health>();
            reposeSet = UnityEngine.Random.Range(minRepose, maxRepose);
            energy = GetComponent<Energy>();
            mover = GetComponent<Mover>();

        }

        // Update is called once per frame
        void Update()
        {
            if (timeSinceRepose > reposeSet)
            {
                PerformTwitch();
            }

            energy.ReduceEnergy(fatigue);

            if (health.IsDead()) return;

            if (PerformCombat()) return;
            if (PerformMovement()) return;


            timeSinceRepose += Time.deltaTime;

        }

        public void OnConversationStart(Transform actor)
        {
            FindObjectOfType<Pause>().PauseGame();
            Debug.Log("Frozen!");
        }

        public void OnConversationEnd(Transform actor)
        {
            FindObjectOfType<Pause>().UnpauseGame();
            Debug.Log("Annnnnd, Unfreeze!");
        }

        private bool PerformCombat()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(GetMouseRay(), Vector2.zero, 0);
            foreach (RaycastHit2D hit in hits)
            {

                CombatTarget target = hit.transform.root.GetComponent<CombatTarget>();

                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }


                return true;

            }

            return false;

        }

        private bool PerformMovement()
        {

            if (FindObjectOfType<Pause>().GamePaused()) return false;

            CauseFatigue();
            RaycastHit2D hit = Physics2D.Raycast(GetMouseRay(), Vector2.zero, 0);
            if (hit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMovement(GetMouseRay(), false);
                }

                CheckRun();
                return true;
            }
            return false;           

        }

        private void CauseFatigue()
        {
            if (mover.IsMoving())
            {
                if (CheckRun())
                    fatigue = runFatigue;
                else
                    fatigue = walkFatigue;
            }
            else
                fatigue = idleFatigue;
        }

        private void PerformTwitch()
        {
            mover.ReposeIdle();
            timeSinceRepose = 0f;
            reposeSet = UnityEngine.Random.Range(minRepose, maxRepose);
       
        }

        private static Vector2 GetMouseRay()
        {
            //Debug.Log("Getting the mouse ray: " + Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString());
            return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        }

        private bool CheckRun()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                mover.SetRun(true);
                return true;
            }
            else
            {
                mover.SetRun(false);
                return false;
            }
        }




    }
}
