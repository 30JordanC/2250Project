using Player;
using UnityEngine;

namespace Level6Scripts
{
    public class LavaDeath : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            // Kill player
            Health ph = other.GetComponent<Health>()
                        ?? other.GetComponentInParent<Health>()
                        ?? other.GetComponentInChildren<Health>();
            if (ph != null)
            {
                ph.TakeDamage(9999f);
                return;
            }

            // Kill enemy
            EnemyHealth eh = other.GetComponent<EnemyHealth>()
                             ?? other.GetComponentInParent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(9999f);
                return;
            }
        }
    }
}