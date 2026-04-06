using UnityEngine;

namespace Level6Scripts
{
    public class LavaDamageLevel6 : MonoBehaviour
    {
        public float damageAmount = 999f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            SoundManager.Instance?.PlaySFX(SoundManager.SFX.LavaDeath);
            Debug.Log($"Player fell in lava! Damage: {damageAmount}");

            // Hook to your player health script:
            // PlayerHealth ph = other.GetComponent<PlayerHealth>();
            // if (ph != null) ph.TakeDamage(damageAmount);
        }
    }
}