using Arena.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Arena.AI
{
    public class AttackState : State<EnemyController>
    {
        private Animator animator;
        private AttackStateController attackStateController;
        private IAttackable attackable;
        protected int attackTriggerHash = Animator.StringToHash("AttackTrigger");
        protected int attackIndexHash = Animator.StringToHash("AttackIndex");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            attackStateController = context.GetComponent<AttackStateController>();
            attackable = context.GetComponent<IAttackable>();
        }

        public override void OnEnter()
        {
            if (attackable == null || attackable.CurrentAttackBehaviour == null)
            {
                stateMachine.ChangeState<IdleState>();
                return;
            }
            attackable.CurrentAttackBehaviour.animationIndex = Random.Range(0, 3);
            attackStateController.enterAttackHandler += OnEnterAttackState;
            attackStateController.exitAttackHandler += OnExitAttackState;

            animator?.SetInteger(attackIndexHash, attackable.CurrentAttackBehaviour.animationIndex);
            animator?.SetTrigger(attackTriggerHash);
       
            attackStateController.OnCheckAttackCollider(attackable.CurrentAttackBehaviour.animationIndex);
        }

        public override void Update(float deltaTime)
        {
        }

        public override void OnExit()
        {
            attackStateController.enterAttackHandler -= OnEnterAttackState;
            attackStateController.exitAttackHandler -= OnExitAttackState;
            

        }

        public void OnEnterAttackState()
        {
       
        }

        public void OnExitAttackState()
        {
            stateMachine.ChangeState<IdleState>();
        }
    }

}