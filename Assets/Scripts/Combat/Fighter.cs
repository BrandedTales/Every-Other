using UnityEngine;
using System.Collections;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health target;
        Mover myMover;

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 15f;
        [SerializeField] float timeBetweenAttacks = 1f;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            myMover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target != null)
            {
                if (myMover.GetMoveTarget() == null) return ;
                

                Vector2 distanceVector = target.transform.position - transform.position;
                if (distanceVector.magnitude >= weaponRange)
                {    
                    myMover.StartMovement(new Vector2(target.transform.position.x, target.transform.position.y), true);                  
                }
                else
                {
                    myMover.Cancel();
                    StrikeEnemy();                            
                }
                
            }
        }

        private void StrikeEnemy()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponentInChildren<Animator>().ResetTrigger("Attack");
            GetComponentInChildren<Animator>().SetTrigger("Attack");  //This will trigger the "Hit" method in mid animation
        }

        public void Attack(GameObject combatTarget)
        {
            //Debug.Log(gameObject.name + " attack!!!");

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
            Debug.Log(gameObject.name + " targetting " + target.name);
        }

        public void Cancel()
        {
            GetComponentInChildren<Animator>().ResetTrigger("NoTarget");
            GetComponentInChildren<Animator>().SetTrigger("NoTarget");
            target = null;
        }


        //Used by Animator
        public void Hit()
        {
            if (target==null)
            {
                return;
            }

            target.TakeDamage(weaponDamage);

            if (target.IsDead())
            {
                Cancel();
            }
        
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest != null) && !targetToTest.IsDead();

        }

    }
}