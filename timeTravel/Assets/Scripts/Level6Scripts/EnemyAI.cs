using UnityEngine;

namespace Level6Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Target")]
        public Transform player;

        [Header("Movement")]
        public float moveSpeed = 2.5f;
        public float chaseRange = 10f;
        public float attackRange = 2f;
        public float rotationSpeed = 8f;

        [Header("Attack")]
        public float attackDamage = 15f;
        public float attackCooldown = 1.2f;

        [Header("Optional Patrol")]
        public bool patrol;
        public Transform[] patrolPoints;
        public float patrolWaitTime = 1.5f;

        [Header("Optional")]
        public Animator animator;

        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");

        private int _currentPatrolIndex;
        private float _lastAttackTime = -999f;
        private float _patrolWaitTimer;
        private EnemyHealth _enemyHealth;

        private void Start()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            _enemyHealth = GetComponent<EnemyHealth>();

            // Start searching for player repeatedly until found.
            // This handles players that are imported/spawned at runtime
            // so spawn order does not matter.
            InvokeRepeating(nameof(FindPlayer), 0f, 1f);
        }

        private void FindPlayer()
        {
            // Already have a player reference, stop searching
            if (player != null)
            {
                CancelInvoke(nameof(FindPlayer));
                return;
            }

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                CancelInvoke(nameof(FindPlayer));
                Debug.Log("EnemyAI: Player found and locked on.");
            }
        }

        private void Update()
        {
            if (_enemyHealth != null && _enemyHealth.IsDead()) return;
            if (player == null) return;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange)
                AttackPlayer();
            else if (distance <= chaseRange)
                ChasePlayer();
            else if (patrol && patrolPoints != null && patrolPoints.Length > 0)
                Patrol();
            else
                SetMoveAnimation(false);
        }

        private void ChasePlayer()
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            transform.position += transform.forward * (moveSpeed * Time.deltaTime);
            SetMoveAnimation(true);
        }

        private void Patrol()
        {
            Transform patrolTarget = patrolPoints[_currentPatrolIndex];
            Vector3 direction = patrolTarget.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude > 0.2f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );

                transform.position += transform.forward * (moveSpeed * Time.deltaTime);
                SetMoveAnimation(true);
            }
            else
            {
                SetMoveAnimation(false);
                _patrolWaitTimer += Time.deltaTime;

                if (_patrolWaitTimer >= patrolWaitTime)
                {
                    _patrolWaitTimer = 0f;
                    _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
                }
            }
        }

        private void AttackPlayer()
        {
            SetMoveAnimation(false);

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            if (Time.time >= _lastAttackTime + attackCooldown)
            {
                _lastAttackTime = Time.time;

                if (animator != null)
                    animator.SetTrigger(AttackHash);

                // Play golem attack sound
                SoundManager.Instance?.PlaySFXAt(SoundManager.SFX.GolemAttack, transform.position);

                // Hook to your player health script here, for example:
                // PlayerHealth ph = player.GetComponent<PlayerHealth>();
                // if (ph != null) ph.TakeDamage(attackDamage);
                Debug.Log($"EnemyAI: Golem attacked player for {attackDamage} damage.");
            }
        }

        private void SetMoveAnimation(bool moving)
        {
            if (animator != null)
                animator.SetBool(IsMovingHash, moving);
        }
    }
}