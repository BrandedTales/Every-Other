using UnityEngine;
using System.Collections;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        CombatTarget target;
        Health health;

        [SerializeField] float minRepose = 5f;
        [SerializeField] float maxRepose = 8f;

        float reposeSet = 0f;
        float timeSinceRepose = 0f;

        // Use this for initialization
        void Start()
        {
            health = GetComponent<Health>();
            reposeSet = UnityEngine.Random.Range(minRepose, maxRepose);

        }

        // Update is called once per frame
        void Update()
        {
            if (timeSinceRepose > reposeSet)
            {
                PerformTwitch();
            }
            if (health.IsDead()) return;

            if (PerformCombat()) return;
            if (PerformMovement()) return;


            timeSinceRepose += Time.deltaTime;

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
                    Debug.Log("Hit somebody that is a target!");
                    GetComponent<Fighter>().Attack(target.gameObject);
                }


                return true;

            }

            return false;

        }

        private bool PerformMovement()
        {
            Debug.Log("Click to move.");
            RaycastHit2D hit = Physics2D.Raycast(GetMouseRay(), Vector2.zero, 0);
            if (hit)
            {
                Debug.Log("Raycast hit something: " + hit.ToString());
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMovement(GetMouseRay(), false);
                    Debug.Log("Move out!");
                }
                CheckRun();
                return true;
            }
            return false;           

        }
        private void PerformTwitch()
        {
            Debug.Log("Call the repose!");
            GetComponent<Mover>().ReposeIdle();
            timeSinceRepose = 0f;
            reposeSet = UnityEngine.Random.Range(minRepose, maxRepose);
       
        }

        private static Vector2 GetMouseRay()
        {
            //Debug.Log("Getting the mouse ray: " + Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString());
            return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        }

        private void CheckRun()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                GetComponent<Mover>().SetRun(true);
            }
            else
            {
                GetComponent<Mover>().SetRun(false);
            }
        }




    }
}
