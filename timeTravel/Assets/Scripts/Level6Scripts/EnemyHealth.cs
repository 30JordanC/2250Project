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

        [Header("Boss Settings")]
        public bool isBoss;

        private static readonly int HurtHash = Animator.StringToHash("Hit");
        private static readonly int IsDeadHash = Animator.StringToHash("isDead");

        private bool _isDead;

        private void Start()
        {
            currentHealth = maxHealth;

            if (animator == null)
                animator = GetComponent<Animator>();

            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            if (animator != null)
                animator.SetTrigger(HurtHash);

            SoundManager.Instance?.PlaySFXAt(SoundManager.SFX.GolemHurt, transform.position);

            Debug.Log($"EnemyHealth: {gameObject.name} took {amount} damage! Health: {currentHealth}");

            if (currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            _isDead = true;

            // Disable EnemyAI if present
            EnemyAI enemyAI = GetComponent<EnemyAI>();
            if (enemyAI != null)
                enemyAI.enabled = false;

            if (animator != null)
                animator.SetBool(IsDeadHash, true);

            // Play death sounds
            SoundManager.Instance?.PlaySFXAt(SoundManager.SFX.GolemDeath, transform.position);
            SoundManager.Instance?.CrossfadeMusic(null);
            SoundManager.Instance?.PlayVictoryMusic();

            // Trigger boss defeated if this is the boss
            if (isBoss && Level6Manager.Instance != null)
                Level6Manager.Instance.BossDefeated();

            // Destroy after delay if enabled
            if (destroyOnDeath)
                Destroy(gameObject, destroyDelay);
        }

        public bool IsDead() => _isDead;
    }
}