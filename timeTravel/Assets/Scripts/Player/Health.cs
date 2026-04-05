using Level2Scripts;
using UnityEngine;

namespace Player
{
    public class Health : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float currentHealth;
        public float passiveHealRate;
        public float healthRegenDelay;
        private float _lastDamagedTime;
        public DeathUI deathUI;
        private bool _isDead;

        void Start()
        {
            _isDead = false;
            currentHealth = maxHealth;

            if (deathUI == null)
                deathUI = FindFirstObjectByType<DeathUI>();
        }

        void Update()
        {
            if (_isDead) return;

            if (Time.time > _lastDamagedTime + healthRegenDelay && currentHealth < maxHealth)
                Heal(passiveHealRate * Time.deltaTime);

            if (deathUI == null)
                deathUI = FindFirstObjectByType<DeathUI>();
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            _lastDamagedTime = Time.time;

            Debug.Log($"Player took {amount} damage! Health: {currentHealth}");

            if (currentHealth <= 0f)
                Die();
        }

        public void Heal(float amount)
        {
            if (_isDead) return;

            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        public void Die()
        {
            if (_isDead) return;
            _isDead = true;

            Debug.Log("Player died!");

            if (deathUI == null)
                deathUI = FindFirstObjectByType<DeathUI>();

            if (deathUI != null)
                deathUI.ShowDeathScreen();
            else
                Debug.LogWarning("Health: DeathUI not found in scene!");
        }

        public void ResetHealth()
        {
            _isDead = false;
            currentHealth = maxHealth;
            Debug.Log("Health: Player health reset!");
        }
    }
}