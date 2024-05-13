using Arena.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arena.AI
{
    [Serializable]
    public class IdleState : State<EnemyController>
    {
        bool isPatrol = false;
        private float minIdleTime = 0.0f;
        private float maxIdleTime = 2.0f;
        private float idleTime = 0.0f;

        private Animator animator;
        private CharacterController controller;

        protected int isMoveHash = Animator.StringToHash("IsMove");
        protected int moveSpeedHash = Animator.StringToHash("MoveSpeed");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
        }

        public override void OnEnter()
        {
            animator?.SetBool(isMoveHash, false);
            animator.SetFloat(moveSpeedHash, 0);
            controller?.Move(Vector3.zero);

            if (context is EnemyController_Range)
            {
                isPatrol = true;
                idleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
            }
        }

        public override void Update(float deltaTime)
        {
            // if searched target
            // change to move state
            if (context.Target)
            {
                if (context.IsAvailableAttack)
                {
                    // check attack cool time
                    // and transition to attack state
                    stateMachine.ChangeState<AttackState>();
                }
                else
                {
                    stateMachine.ChangeState<MoveState>();
                }
            }
            else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime)
            {
                stateMachine.ChangeState<MoveToWayPoint>();
            }
            else
            {
                stateMachine.ChangeState<IdleState>();
            }
        }

        public override void OnExit()
        {
        }
    }
}