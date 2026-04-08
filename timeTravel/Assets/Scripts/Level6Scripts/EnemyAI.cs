using Player;
using UnityEngine;

namespace Level6Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Target (auto-found — do not assign manually)")]
        public Transform player;

        [Header("Movement")]
        public float moveSpeed = 2.5f;
        public float chaseRange = 30f;
        public float attackRange = 2f;
        public float rotationSpeed = 8f;

        [Header("Attack")]
        public float attackDamage = 15f;
        public float attackCooldown = 1.2f;

        [Header("Optional Patrol")]
        public bool patrol;
        public Transform[] patrolPoints;
        public float patrolWaitTime = 1.5f;

        [Header("Optional — drag Animator component here")]
        public Animator animator;

        // Correct parameter names from GolemAnimator
        private static readonly int WalkHash   = Animator.StringToHash("Walk");
        private static readonly int DamageHash = Animator.StringToHash("Damage");
        private static readonly int IsDeadHash = Animator.StringToHash("isDead");

        private int _currentPatrolIndex;
        private float _lastAttackTime = -999f;
        private float _patrolWaitTimer;
        private EnemyHealth _enemyHealth;
        private Rigidbody _rb;

        private void Start()
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            _enemyHealth = GetComponent<EnemyHealth>();
            _rb = GetComponent<Rigidbody>();

            // Lock rotation so enemy doesn't tip over
            if (_rb != null)
            {
                _rb.freezeRotation = true;
                _rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

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

            // 2. Grab directly from SceneTransitionManager
            if (SceneTransitionManager.Instance != null &&
                SceneTransitionManager.Instance.playerRoot != null)
            {
                player = SceneTransitionManager.Instance.playerRoot.transform;
                CancelInvoke(nameof(FindPlayer));
                Debug.Log("EnemyAI: Player found via SceneTransitionManager — " + player.name);
                return;
            }

            // 3. Fallback: search by common player object names
            foreach (string n in new[] { "Player", "PlayerObject", "PlayerRoot", "PlayerCharacter" })
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
                SetWalkAnimation(0f);
        }

        private void FixedUpdate()
        {
            if (_rb != null)
            {
                // Keep enemy upright always
                _rb.rotation = Quaternion.Euler(0f, _rb.rotation.eulerAngles.y, 0f);
            }
        }

        private void ChasePlayer()
        {
            if (player == null) return;

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f) return;

            // Rotate toward player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Move using Rigidbody if available, otherwise transform
            if (_rb != null)
            {
                Vector3 velocity = transform.forward * moveSpeed;
                _rb.linearVelocity = new Vector3(velocity.x, _rb.linearVelocity.y, velocity.z);
            }
            else
            {
                transform.position += transform.forward * (moveSpeed * Time.deltaTime);
            }

            SetWalkAnimation(1f);
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

                if (_rb != null)
                {
                    Vector3 velocity = transform.forward * moveSpeed;
                    _rb.linearVelocity = new Vector3(velocity.x, _rb.linearVelocity.y, velocity.z);
                }
                else
                {
                    transform.position += transform.forward * (moveSpeed * Time.deltaTime);
                }

                SetWalkAnimation(1f);
            }
            else
            {
                SetWalkAnimation(0f);
                if (_rb != null)
                    _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);

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
            SetWalkAnimation(0f);

            // Stop moving when attacking
            if (_rb != null)
                _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);

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
                    animator.SetTrigger(DamageHash);

                SoundManager.Instance?.PlaySFXAt(SoundManager.SFX.GolemAttack, transform.position);

                Health ph = player.GetComponentInChildren<Health>()
                             ?? player.GetComponent<Health>();
                if (ph != null)
                    ph.TakeDamage(attackDamage);
                else
                    Debug.Log("EnemyAI: No Health found on player.");
            }
        }

        private void SetWalkAnimation(float value)
        {
            if (animator != null)
                animator.SetFloat(WalkHash, value);
        }

        public void OnDeath()
        {
            CancelInvoke(nameof(FindPlayer));

            if (_rb != null)
                _rb.linearVelocity = Vector3.zero;

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