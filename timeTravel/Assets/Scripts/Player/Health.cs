using Level2Scripts;
using UnityEngine;
using UnityEngine.SceneManagement; // ✅ ADDED

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

        // ✅ ADDED
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // ✅ ADDED
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // ✅ ADDED
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Try to find DeathUI in the new scene
            DeathUI foundUI = FindObjectOfType<DeathUI>();

            if (foundUI != null)
            {
                deathUI = foundUI;
                Debug.Log("DeathUI reattached after scene load");
            }
            else
            {
                Debug.LogWarning("DeathUI not found in scene!");
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            currentHealth = maxHealth;

            // ✅ ADDED (fallback in case sceneLoaded doesn't trigger first time)
            if (deathUI == null)
            {
                deathUI = FindObjectOfType<DeathUI>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > _lastDamagedTime + healthRegenDelay && currentHealth < maxHealth)
            {
                Heal(passiveHealRate*Time.deltaTime);
            }
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        public void Die()
        {
            Debug.Log("Player died");

            if (deathUI != null)
            {
                deathUI.ShowDeathScreen();
            }
        }
    }
}