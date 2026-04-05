using UnityEngine;
using Player;
using Level6Scripts;

namespace Level6Scripts
{
    

    public class LavaDeath : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // Kill player
            Health ph = other.GetComponent<Health>()
                        ?? other.GetComponentInParent<Health>()
                        ?? other.GetComponentInChildren<Health>();
            if (ph != null)
            {
                ph.TakeDamage(9999f);
                Debug.Log("Player fell in lava and died!");
                return;
            }

            // Kill enemy
            EnemyHealth eh = other.GetComponent<EnemyHealth>()
                             ?? other.GetComponentInParent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(9999f);
                Debug.Log("Enemy fell in lava and died!");
                return;
            }

            // Destroy anything else that falls in
            if (other.gameObject != gameObject)
            {
                Destroy(other.gameObject);
                Debug.Log($"{other.gameObject.name} destroyed by lava!");
            }
        }
    }
}