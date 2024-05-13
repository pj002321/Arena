using Arena.AI;
using Arena.Core;
using Arena.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arena.Player;

namespace Arena.Characters
{ 
    public class EnemyController_Patrol : EnemyController, IDamagable
    {
        #region Variables

        public Collider weaponCollider;
        public Transform hitPoint;
        public GameObject hitEffect = null;

        public Transform[] waypoints;

        //public float maxHealth = 100f;
        //public float currentHealth = 100f;

        #endregion Variables

        #region Proeprties



        #endregion Properties

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveState());
            //stateMachine.AddState(new AttackState());
            stateMachine.AddState(new MoveToWayPoint());

            health = maxHealth;


        }

        #endregion Unity Methods

        #region Helper Methods

        public override bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                {
                    return false;
                }

                float distance = Vector3.Distance(transform.position, Target.position);
                return (distance <= AttackRange);
            }
        }

        public void EnableAttackCollider()
        {
            Debug.Log("Check Attack Event");
            if (weaponCollider)
            {
                weaponCollider.enabled = true;
            }


            StartCoroutine("DisableAttackCollider");
        }

        IEnumerator DisableAttackCollider()
        {
            yield return new WaitForFixedUpdate();

            if (weaponCollider)
            {
                weaponCollider.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != weaponCollider)
            {
                return;
            }

            if (((1 << other.gameObject.layer) & TargetMask) != 0)
            {
                //It matched one
                Debug.Log("Attack Trigger: " + other.name);
                PlayerController playerCharacter = other.gameObject.GetComponent<PlayerController>();
               

            }

            if (((1 << other.gameObject.layer) & TargetMask) == 0)
            {
                //It wasn't in an ignore layer
            }
        }

        #endregion Helper Methods

        #region IDamagable interfaces

        public float maxHealth = 100f;

        private float health;

        public bool IsAlive => (health > 0);

        private int hitTriggerHash = Animator.StringToHash("HitTrigger");
        private int isAliveHash = Animator.StringToHash("IsAlive");

        public void TakeDamage(int damage, GameObject hitEffectPrefab)
        {
            if (!IsAlive)
            {
                return;
            }

            health -= damage;

            if (hitEffectPrefab)
            {
                Instantiate(hitEffectPrefab, hitPoint);
            }

            if (IsAlive)
            {
                animator?.SetTrigger(hitTriggerHash);
            }
            else
            {
                animator?.SetBool(isAliveHash, false);

                Destroy(gameObject, 3.0f);
            }
        }
        #endregion IDamagable interfaces
    }
}
