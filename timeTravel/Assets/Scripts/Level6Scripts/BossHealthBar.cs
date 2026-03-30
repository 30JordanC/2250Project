using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{


    public class EnemyHealthBarUI : MonoBehaviour
    {
        public EnemyHealth enemyHealth;
        public Slider slider;

        private void Update()
        {
            if (enemyHealth != null)
            {
                slider.maxValue = enemyHealth.maxHealth;
                slider.value = enemyHealth.currentHealth;
            }
        }
    }
}