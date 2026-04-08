using Player;
using UnityEngine;

namespace Level6Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Target (auto-found — do not assign manually)")]
        public Transform player;

        [Header("Movement")]
        public float moveSpeed     = 2.5f;
        public float chaseRange    = 30f;
        public float attackRange   = 2f;
        public float rotationSpeed = 8f;

        [Header("Attack")]
        public float attackDamage   = 15f;
        public float attackCooldown = 1.2f;

        [Header("Optional Patrol")]
        public bool patrol;
        public Transform[] patrolPoints;
        public float patrolWaitTime = 1.5f;

        [Header("Optional — drag Animator component here")]
        public Animator animator;

        private static readonly int AttackHash   = Animator.StringToHash("Attack");
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
        private static readonly int IsDeadHash   = Animator.StringToHash("isDead");

        private int         _currentPatrolIndex;
        private float       _lastAttackTime = -999f;
        private float       _patrolWaitTimer;
        private EnemyHealth _enemyHealth;

        // ─────────────────────────────────────────────────────────────

        private void Start()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            _enemyHealth = GetComponent<EnemyHealth>();

            InvokeRepeating(nameof(FindPlayer), 0f, 0.5f);
        }

        private void FindPlayer()
        {
            if (player != null)
            {
                CancelInvoke(nameof(FindPlayer));
                return;
            }

            // 1. Try by Player tag
            GameObject byTag = GameObject.FindGameObjectWithTag("Player");
            if (byTag != null)
            {
                player = byTag.transform;
                CancelInvoke(nameof(FindPlayer));
                Debug.Log("EnemyAI: Player found by tag — " + player.name);
                return;
            }

            // 2. Grab directly from SceneTransitionManager (most reliable for your setup)
            if (SceneTransitionManager.Instance != null &&
                SceneTransitionManager.Instance.playerRoot != null)
            {
                player = SceneTransitionManager.Instance.playerRoot.transform;
                CancelInvoke(nameof(FindPlayer));
                Debug.Log("EnemyAI: Player found via SceneTransitionManager — " + player.name);
                return;
            }

            // 3. Fallback: search by common player object names
            foreach (string n in new[] { "Player", "PlayerObject", "NewPlayerObject", "PlayerCharacter" })
            {
                GameObject g = GameObject.Find(n);
                if (g != null)
                {
                    player = g.transform;
                    CancelInvoke(nameof(FindPlayer));
                    Debug.Log("EnemyAI: Player found by name — " + n);
                    return;
                }
            }

            Debug.Log("EnemyAI: Searching for player...");
        }

        // ─────────────────────────────────────────────────────────────

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

                SoundManager.Instance?.PlaySFXAt(SoundManager.SFX.GolemAttack, transform.position);

                // Deal damage to player
                Health ph = player.GetComponentInChildren<Health>()
                                     ?? player.GetComponent<Health>();
                if (ph != null)
                    ph.TakeDamage(attackDamage);
                else
                    Debug.Log("EnemyAI: No PlayerHealthLevel6 on player — add it to your Player GameObject.");
            }
        }

        private void SetMoveAnimation(bool moving)
        {
            if (animator != null)
                animator.SetBool(IsMovingHash, moving);
        }

        public void OnDeath()
        {
            CancelInvoke(nameof(FindPlayer));
            if (animator != null)
                animator.SetBool(IsDeadHash, true);
            enabled = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}