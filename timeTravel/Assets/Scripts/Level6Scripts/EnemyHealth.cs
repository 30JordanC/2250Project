using UnityEngine;

namespace Level6Scripts
{
    public class EnemyHealth : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float currentHealth;

        [Header("Optional")]
        public Animator animator;
        public bool destroyOnDeath;
        public float destroyDelay = 2f;

        // FIX: Added isBoss flag. When true, dying calls Level6Manager.BossDefeated()
        // so the terra pickup spawns and the level can be completed.
        [Header("Boss Settings")]
        public bool isBoss;

        private static readonly int HurtHash = Animator.StringToHash("Hurt");
        private static readonly int IsDeadHash = Animator.StringToHash("isDead");

        private bool _isDead;

        private void Start()
        {
            currentHealth = maxHealth;

            if (animator == null)
                animator = GetComponent<Animator>();
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            if (animator != null)
                animator.SetTrigger(HurtHash);

            if (currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            _isDead = true;

            // Disable AI so the golem stops moving
            EnemyAI enemyAI = GetComponent<EnemyAI>();
            if (enemyAI != null)
                enemyAI.enabled = false;

            if (animator != null)
                animator.SetBool(IsDeadHash, true);

            // FIX: Was missing entirely. Now notifies Level6Manager when boss dies
            // so terraObject gets activated and level completion becomes possible.
            if (isBoss && Level6Manager.Instance != null)
                Level6Manager.Instance.BossDefeated();

            if (destroyOnDeath)
                Destroy(gameObject, destroyDelay);
        }

        public bool IsDead()
        {
            return _isDead;
        }
    }
}