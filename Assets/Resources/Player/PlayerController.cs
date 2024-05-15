using Arena.AI;
using Arena.Characters;
using Arena.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Arena.Player
{
    [RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour, IAttackable, IDamagable
    {
        #region Variables
        public PlaceTargetWithMouse picker;

        private CharacterController controller;
        [SerializeField]
        private LayerMask groundLayerMask;

        private NavMeshAgent agent;
        private Camera camera;
        public ParticleSystem cursorEffect;

        [SerializeField]
        private Animator animator;

        private float leftoverDist = 1.5f;
        
        readonly int moveHash = Animator.StringToHash("Move");
        readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
        readonly int fallingHash = Animator.StringToHash("Falling");
        readonly int attackTriggerHash = Animator.StringToHash("AttackTrigger");
        readonly int attackIndexHash = Animator.StringToHash("AttackIndex");
        readonly int hitTriggerHash = Animator.StringToHash("HitTrigger");
        readonly int isAliveHash = Animator.StringToHash("IsAlive");

        [SerializeField]
        private LayerMask targetMask;
        public Transform target;

        public bool IsInAttackState => GetComponent<AttackStateController>()?.IsInAttackState ?? false;

        [SerializeField]
        private Transform hitPoint;

        public float maxHealth = 100f;
        protected float health;

        #endregion Variables

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;

            camera = Camera.main;

            health = maxHealth;

            InitAttackBehaviour();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsAlive)
            {
                return;
            }

            CheckAttackBehaviour();

            //calcAttackCoolTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                AttackTarget();
            }
            // Process mouse left button input
            if (Input.GetMouseButtonDown(1) /*&& !IsInAttackState*/)
            {
                // Make ray from screen to world
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                // Check hit from ray
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, groundLayerMask)) {
                    Debug.Log("We hit " + hit.collider.name + " " + hit.point);
                    RemoveTarget();
                    agent.SetDestination(hit.point);
                    if (picker) {
                        picker.SetPosition(hit);
                    }
                    if (cursorEffect != null) {
                        ParticleSystem effectInstance = Instantiate(cursorEffect, hit.point += new Vector3(0, 0.3f, 0), hit.collider.transform.rotation);
                        Destroy(effectInstance.gameObject, 2f);
                    }
                }
            }
        

            if (target != null)
            {
                if (!(target.GetComponent<IDamagable>()?.IsAlive ?? false))
                {
                    RemoveTarget();
                }
                else
                {
                    agent.SetDestination(target.position);
                    FaceToTarget();
                }
            }

            if ((agent.remainingDistance > agent.stoppingDistance + leftoverDist))
            {
                controller.Move(agent.velocity * Time.deltaTime);
                animator.SetBool(moveHash, true);
            }
            else
            {
                //controller.Move(agent.velocity * Time.deltaTime);

                if (!agent.pathPending)
                {
                    animator.SetBool(moveHash, false);
                    agent.ResetPath();
                }
            }

        }

        private void OnAnimatorMove()
        {
            Vector3 position = transform.position;
            position.y = agent.nextPosition.y;

            animator.rootPosition = position;
            agent.nextPosition = position;
        }

        #region Helper Methods
        private void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                behaviour.targetMask = targetMask;
            }
        }

        private void CheckAttackBehaviour()
        {
            if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
            {
                CurrentAttackBehaviour = null;

                foreach (AttackBehaviour behaviour in attackBehaviours)
                {
                    if (behaviour.IsAvailable)
                    {
                        if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority))
                        {
                            CurrentAttackBehaviour = behaviour;
                        }
                    }
                }
            }
        }

        void SetTarget(Transform newTarget)
        {
            target = newTarget;

            //agent.stoppingDistance = CurrentAttackBehaviour?.range ?? 0;
            //agent.updateRotation = false;
            //agent.SetDestination(newTarget.transform.position);
        }

        void RemoveTarget()
        {
            target = null;
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;
            agent.ResetPath();
        }

        void AttackTarget()
        {
            animator.SetInteger(attackIndexHash, CurrentAttackBehaviour.animationIndex);
            animator.SetTrigger(attackTriggerHash);
            CurrentAttackBehaviour.animationIndex = Random.Range(0, 3);

            if (CurrentAttackBehaviour == null)
            {
                return;
            }

            if (target != null && !IsInAttackState && CurrentAttackBehaviour.IsAvailable)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= CurrentAttackBehaviour?.range)
                {
                    controller.Move(Vector3.zero);
                }
            }
        }

        void FaceToTarget()
        {
            if (target)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20.0f);
            }
        }

        #endregion Helper Methods

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
            if (CurrentAttackBehaviour != null)
            {
                CurrentAttackBehaviour.ExecuteAttack(target.gameObject);
            }
        }

        #endregion IAttackable Interfaces

        #region IDamagable Interfaces

        public bool IsAlive => health > 0;

        public void TakeDamage(int damage, GameObject damageEffectPrefab)
        {
            if (!IsAlive)
            {
                return;
            }

            health -= damage;

            if (damageEffectPrefab)
            {
                Instantiate<GameObject>(damageEffectPrefab, hitPoint);
            }

            if (IsAlive)
            {
                animator?.SetTrigger(hitTriggerHash);
            }
            else
            {
                animator?.SetBool(isAliveHash, false);
            }
        }

        #endregion IDamagable Interfaces
    }


}


