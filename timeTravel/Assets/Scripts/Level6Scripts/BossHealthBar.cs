using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{
    // FIX: Class was named EnemyHealthBarUI but the file is called BossHealthBar.
    // Renamed to BossHealthBar to match the filename, avoiding Unity confusion.
    public class BossHealthBar : MonoBehaviour
    {
        public EnemyHealth enemyHealth;
        public Slider slider;

        private void Start()
        {
            // FIX: Added null checks on Start so missing references give a
            // clear warning instead of a silent broken UI.
            if (enemyHealth == null)
                Debug.LogWarning("BossHealthBar: enemyHealth is not assigned in Inspector.");

            if (slider == null)
                Debug.LogWarning("BossHealthBar: slider is not assigned in Inspector.");
        }

        private void Update()
        {
            if (enemyHealth == null || slider == null) return;

            slider.maxValue = enemyHealth.maxHealth;
            slider.value = enemyHealth.currentHealth;

            // Hide the health bar when the boss is dead
            if (enemyHealth.IsDead())
                gameObject.SetActive(false);
        }
    }
}