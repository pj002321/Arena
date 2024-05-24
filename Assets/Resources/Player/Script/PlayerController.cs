using Arena.AI;
using Arena.Characters;
using Arena.Core;
using Arena.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
        public ManualCollision attackCollision;
        private NavMeshAgent agent;
        private Camera camera;
        public ParticleSystem cursorEffect;
        public ParticleSystem TrailEffect;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private PlayerBattleUI battleUI;
        public float maxHealth = 100f;
        protected float health;

        private float leftoverDist = 1.0f;

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
        public Transform Trail;

        public bool IsInAttackState => GetComponent<AttackStateController>()?.IsInAttackState ?? false;

        [SerializeField]
        private Transform hitPoint;

        #endregion Variables

        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;

            camera = Camera.main;

            health = maxHealth;
            if (battleUI)
            {
                battleUI.MinimumValue = 0.0f;
                battleUI.MaximumValue = maxHealth;
                battleUI.Value = health;
            }
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
                agent.velocity=Vector3.zero;
                AttackTarget();
                RemoveTarget();
            }
            // Process mouse left button input
            if (Input.GetMouseButtonDown(1) /*&& !IsInAttackState*/)
            {
                // Make ray from screen to world
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                // Check hit from ray
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
                {
                    //Debug.Log("We hit " + hit.collider.name + " " + hit.point);
                    RemoveTarget();
                    agent.SetDestination(hit.point);
                    if (picker)
                    {
                        picker.SetPosition(hit);
                    }
                    if (cursorEffect != null)
                    {
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
                controller.Move(Vector3.zero);

                if (!agent.pathPending)
                {
                    animator.SetBool(moveHash, false);
                    agent.ResetPath();
                }
            }

        }

        private void OnAnimatorMove()
        {
            Vector3 position = agent.nextPosition;
            animator.rootPosition = agent.nextPosition;
            transform.position = position;
        }

        #endregion Unity Methods
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
        #region AttackMotion
        void AttacktoDamageCheck()
        {
            Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);

            foreach (var hitCollider in colliders)
            {
                IDamagable damagable = hitCollider.GetComponent<IDamagable>();
                if (damagable != null && damagable.IsAlive)
                {
                    SetTarget(hitCollider.transform);
                    picker.target = hitCollider.transform;
                    int damage = Random.Range(5, 10);
                    damagable.TakeDamage(damage, null);
                }
            }
        }
        void AreaAttacktoDamageCheck()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15, targetMask);

            foreach (var hitCollider in hitColliders)
            {
                IDamagable damagable = hitCollider.GetComponent<IDamagable>();
                if (damagable != null && damagable.IsAlive)
                {

                    int damage = Random.Range(10, 25);
                    damagable.TakeDamage(damage, null);
                }
            }
        }

        #endregion AttackMotion
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
            animator.SetBool(moveHash, false);
            CurrentAttackBehaviour.animationIndex = Random.Range(0, 3);
            animator.SetInteger(attackIndexHash, CurrentAttackBehaviour.animationIndex);
            animator.SetTrigger(attackTriggerHash);

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
            CurrentAttackBehaviour.ExecuteAttack();
        }

        #endregion IAttackable Interfaces

        #region IDamagable Interfaces

        public bool IsAlive => health > 0;
     
        public void TakeDamage(int damage, GameObject damageEffectPrefab)
        {
            Debug.Log("PlayerHit" + health);
            if (!IsAlive)
            {
                return;
            }

            health -= damage;
            if (battleUI)
            {
                battleUI.Value = health;
                battleUI.TakeDamage(damage);
            }
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
                if (battleUI != null)
                {
                    battleUI.enabled = false;
                }
                animator?.SetBool(isAliveHash, false);
            }
        }

        #endregion IDamagable Interfaces
    }


}


