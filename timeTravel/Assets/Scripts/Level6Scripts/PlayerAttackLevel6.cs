using UnityEngine;

namespace Level6Scripts
{
    public class PlayerAttackLevel6 : MonoBehaviour
    {
        [Header("Attack Settings")]
        public float attackRange = 8f;
        public float attackDamage = 25f;
        public LayerMask enemyLayer;

        [Header("Axe Visual")]
        public GameObject axeInHand;

        [Header("Attack Animation")]
        public Animator playerAnimator;

        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private float _lastAttackTime;
        public float attackCooldown = 0.5f;
        private readonly Collider[] _hitBuffer = new Collider[10];

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                Attack();
        }

        private void Attack()
        {
            if (Level6Manager.Instance == null)
            {
                Debug.LogWarning("PlayerAttackLevel6: Level6Manager not found.");
                return;
            }

            if (!Level6Manager.Instance.hasSword)
            {
                Debug.Log("PlayerAttackLevel6: Pick up the axe first!");
                return;
            }

            if (Time.time < _lastAttackTime + attackCooldown) return;
            _lastAttackTime = Time.time;

            SoundManager.Instance?.PlaySFX(SoundManager.SFX.SwordSwing);

            if (playerAnimator != null)
                playerAnimator.SetTrigger(AttackTrigger);

            Debug.Log($"Attacking! Range: {attackRange} Layer: {enemyLayer.value}");

            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                attackRange,
                _hitBuffer,
                enemyLayer
            );

            Debug.Log($"Hit count: {hitCount}");

            bool hitSomething = false;
            for (int i = 0; i < hitCount; i++)
            {
                EnemyHealth enemy = _hitBuffer[i].GetComponent<EnemyHealth>()
                                    ?? _hitBuffer[i].GetComponentInParent<EnemyHealth>()
                                    ?? _hitBuffer[i].GetComponentInChildren<EnemyHealth>();

                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    SoundManager.Instance?.PlaySFXAt(
                        SoundManager.SFX.SwordHit,
                        _hitBuffer[i].transform.position
                    );
                    hitSomething = true;
                    Debug.Log($"Hit {_hitBuffer[i].name} for {attackDamage} damage!");
                }

                // Stun ghost — push it back
                GhostStun ghost = _hitBuffer[i].GetComponent<GhostStun>()
                                  ?? _hitBuffer[i].GetComponentInParent<GhostStun>();
                if (ghost != null)
                    ghost.Stun();
            }

            if (!hitSomething)
                Debug.Log("PlayerAttackLevel6: Hit nothing! Is ghost on Enemy layer?");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}