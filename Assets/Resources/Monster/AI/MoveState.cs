using Arena.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Arena.AI
{
    public class MoveState : State<EnemyController>
    {
        private Animator animator;
        private CharacterController controller;
        private NavMeshAgent agent;

        private int isMovehash = Animator.StringToHash("IsMove");
        private int moveSpeedHash = Animator.StringToHash("MoveSpeed");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();

            agent = context.GetComponent<NavMeshAgent>();
        }

        public override void OnEnter()
        {
            agent.stoppingDistance = context.AttackRange;
            agent?.SetDestination(context.Target.position);

            animator?.SetBool(isMovehash, true);
        }

        public override void Update(float deltaTime)
        {
            if (context.Target)
            {
                agent.SetDestination(context.Target.position);
            }
            

            controller.Move(agent.velocity * Time.deltaTime);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                //animator.SetFloat(moveSpeedHash, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
                animator.SetBool(isMovehash, true);
            }
            else
            {

                if (!agent.pathPending)
                {
                    //animator.SetFloat(moveSpeedHash, 0);
                    animator.SetBool(isMovehash, false);
                    stateMachine.ChangeState<IdleState>();
                    agent.ResetPath();

                }
            }
        }

        public override void OnExit()
        {
            agent.stoppingDistance = 0.0f;
            agent.ResetPath();

            animator?.SetBool(isMovehash, false);
            //animator?.SetFloat(moveSpeedHash, 0.0f);
        }
    }
}
