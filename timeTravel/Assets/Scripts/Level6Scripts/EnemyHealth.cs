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

        private static readonly int HurtHash = Animator.StringToHash("Hurt");
        private static readonly int IsDeadHash = Animator.StringToHash("isDead");

        private bool _isDead;

        private void Start()
        {
            currentHealth = maxHealth;

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            if (animator != null)
            {
                animator.SetTrigger(HurtHash);
            }

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;

            EnemyAI enemyAI = GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.enabled = false;
            }

            if (animator != null)
            {
                animator.SetBool(IsDeadHash, true);
            }

            if (destroyOnDeath)
            {
                Destroy(gameObject, destroyDelay);
            }
        }

        public bool IsDead()
        {
            return _isDead;
        }
    }
}