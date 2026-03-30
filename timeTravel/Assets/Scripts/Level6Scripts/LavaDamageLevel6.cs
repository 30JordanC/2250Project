using UnityEngine;

namespace Level6Scripts
{


    public class LavaDamageLevel6 : MonoBehaviour
    {
        public float damageAmount = 999f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) return;

            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}