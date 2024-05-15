using Arena.AI;
using Arena.Core;
using Arena.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arena.Characters
{
    public class EnemyController_Range : EnemyController, IAttackable, IDamagable
    {
        #region Variables

        [SerializeField]
        public Transform hitPoint;
        public Transform[] waypoints;

        public override float AttackRange => CurrentAttackBehaviour?.range ?? 6.0f;

        public float maxHealth => 100f;
        private float health;

        private int hitTriggerHash = Animator.StringToHash("HitTrigger");

        [SerializeField]
        private Transform projectilePoint;

        #endregion Variables

        #region Proeprties
        public override bool IsAvailableAttack
        {
            get {
                if (!Target) {
                    return false;
                }

                float distance = Vector3.Distance(transform.position, Target.position);
                return (distance <= AttackRange);
            }
        }

        #endregion Properties

        #region Unity Methods

        // State Regist
        protected override void Start()
        {
            base.Start();
          
            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new MoveToWayPoint());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());
            stateMachine.AddState(new IdleState());

            health = maxHealth;

         
            InitAttackBehaviour();
        }

        protected override void Update()
        {
            CheckAttackBehaviour();

            base.Update();
        }

        private void OnAnimatorMove()
        {
            // Follow NavMeshAgent
            //Vector3 position = agent.nextPosition;
            //animator.rootPosition = agent.nextPosition;
            //transform.position = position;

            // Follow CharacterController
            Vector3 position = transform.position;
            position.y = agent.nextPosition.y;

            animator.rootPosition = position;
            agent.nextPosition = position;

            // Follow RootAnimation
            //Vector3 position = animator.rootPosition;
            //position.y = agent.nextPosition.y;

            //agent.nextPosition = position;
            //transform.position = position;
        }

        #endregion Unity Methods

        #region Helper Methods
        private void InitAttackBehaviour() {
            foreach (AttackBehaviour behaviour in attackBehaviours) {
                if (CurrentAttackBehaviour == null) {
                    CurrentAttackBehaviour = behaviour;
                }

                behaviour.targetMask = TargetMask;
            }
        }

        private void CheckAttackBehaviour()
        {
            if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable) {
                CurrentAttackBehaviour = null;

                foreach (AttackBehaviour behaviour in attackBehaviours) {
                    if (behaviour.IsAvailable) {
                        if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority)) {
                            CurrentAttackBehaviour = behaviour;
                            
                        }
                    }
                }
            }
        }

        #endregion Helper Methods

        #region IDamagable interfaces

        public bool IsAlive => (health > 0);

        public void TakeDamage(int damage, GameObject hitEffectPrefab)
        {
            if (!IsAlive) {
                return;
            }

            health -= damage;

            if (hitEffectPrefab) {
                Instantiate(hitEffectPrefab, hitPoint);
            }

            if (IsAlive) {
                animator?.SetTrigger(hitTriggerHash);
            }
            else {
                stateMachine.ChangeState<DeadState>();
            }
        }

        #endregion IDamagable interfaces

        #region IAttackable Interfaces


        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }

        public void OnExecuteAttack(int attackIndex)
        {
            if (CurrentAttackBehaviour != null && Target != null) {
                CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectilePoint);
            }
        }

        #endregion IAttackable Interfaces
    }
}
