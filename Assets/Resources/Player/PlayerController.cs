using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Arena.Player
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        private CharacterController characterController;
        private NavMeshAgent agent;
        private Camera camera;

        public Animator animator;
        public ParticleSystem cursorEffect;

        //private bool isGrounded = false;
        public LayerMask groundLayerMask;
        public float groundCheckDistance = 0.3f;

        // Gravity && Drag Setting
        public float gravity = -9.81f;
        public Vector3 drags;

        readonly int moveHash = Animator.StringToHash("Move");
        readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        readonly int fallingHash = Animator.StringToHash("Falling");
        #endregion Variables

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            agent.updatePosition = false; // agent 이동 시스템을 사용하지 않는다.
            agent.updateRotation = true;
            camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            // Process mouse left button input
            if (Input.GetMouseButtonDown(0))
            {
                // Make ray from screen to world
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                // Check hit from ray
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
                {
                    agent.SetDestination(hit.point);
                    //Debug.Log("We hit " + hit.collider.name + " " + hit.point);
                    if (cursorEffect != null)
                    {
                        // Instantiate the click effect
                        ParticleSystem effectInstance = Instantiate(cursorEffect, hit.point += new Vector3(0, 0.3f, 0), hit.collider.transform.rotation);

                        // Destroy the instantiated effect after 2 seconds
                        Destroy(effectInstance.gameObject, 2f);
                    }
                    // Move our player to what we hit
                }
            }

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                characterController.Move(agent.velocity * Time.deltaTime);
                animator.SetFloat(moveSpeedHash, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
                animator.SetBool(moveHash, true);
            }
            else
            {
                characterController.Move(agent.velocity * Time.deltaTime);
                if (!agent.pathPending)
                {
                    animator.SetFloat(moveSpeedHash, 0);
                    animator.SetBool(moveHash, false);
                    agent.ResetPath();
                }
            }

            if (agent.isOnOffMeshLink)
            {
                animator.SetBool(fallingHash, true);
            }
            else
            {
                animator.SetBool(fallingHash, false);
            }
        }

        private void OnAnimatorMove()
        {
            Vector3 position = agent.nextPosition;
            animator.rootPosition = agent.nextPosition;
            transform.position = position;
            agent.nextPosition = position;
        }
    }


}


