using UnityEngine;

namespace Level6Scripts
{
    public class LavaDamageLevel6 : MonoBehaviour
    {
        public float damageAmount = 999f;

        private void OnTriggerEnter(Collider other)
        {
            // FIX: was "if Player return" which skipped the player entirely.
            // Now only damages the player, ignores everything else.
            if (!other.CompareTag("Player")) return;

            // FIX: was looking for a "Health" component which doesn't exist in
            // your project. EnemyHealth is the health script used here,
            // but the player needs their own health component.
            // Using PlayerHealth interface via a common interface approach:
            // If your player has a component called "PlayerHealth", swap below.
            // For now we log a clear damage call so you can hook it to your player health.
            Debug.Log($"Player hit lava! Damage: {damageAmount}");

            // Hook this to your player health script, for example:
            // PlayerHealth ph = other.GetComponent<PlayerHealth>();
            // if (ph != null) ph.TakeDamage(damageAmount);
        }
    }
}