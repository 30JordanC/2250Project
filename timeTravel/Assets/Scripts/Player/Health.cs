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

        void Start()
        {
            currentHealth = maxHealth;

            // Auto find DeathUI if not assigned
            if (deathUI == null)
                deathUI = FindFirstObjectByType<DeathUI>();
        }

        void Update()
        {
            if (Time.time > _lastDamagedTime + healthRegenDelay && currentHealth < maxHealth)
            {
                Heal(passiveHealRate * Time.deltaTime);
            }

            // Keep trying to find DeathUI if still null
            if (deathUI == null)
                deathUI = FindFirstObjectByType<DeathUI>();
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            _lastDamagedTime = Time.time;

            if (currentHealth <= 0f)
                Die();
        }

        public void Heal(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        public void Die()
        {
            Debug.Log("Player died!");

            // Try finding DeathUI one more time
            if (deathUI == null)
                deathUI = FindFirstObjectByType<DeathUI>();

            if (deathUI != null)
                deathUI.ShowDeathScreen();
            else
                Debug.LogWarning("Health: DeathUI not found in scene!");
        }
    }
}