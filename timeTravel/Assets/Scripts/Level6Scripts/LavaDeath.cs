using UnityEngine;
using Player;
using Level6Scripts;

public class LavaDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"LavaDeath triggered by: {other.gameObject.name}");
        HandleDeath(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleDeath(other);
    }

    private void HandleDeath(Collider other)
    {
        // Skip if this is another trigger or the lava itself
        if (other.isTrigger) return;
        if (other.gameObject == gameObject) return;

        // Kill player
        Health ph = other.GetComponent<Health>()
                    ?? other.GetComponentInParent<Health>()
                    ?? other.GetComponentInChildren<Health>();

        if (ph != null)
        {
            Debug.Log($"LavaDeath: Killing player {other.gameObject.name}!");
            ph.Die();
            return;
        }

        // Kill enemy
        EnemyHealth eh = other.GetComponent<EnemyHealth>()
                         ?? other.GetComponentInParent<EnemyHealth>();

        if (eh != null)
        {
            Debug.Log($"LavaDeath: Killing enemy {other.gameObject.name}!");
            eh.TakeDamage(9999f);
            return;
        }
    }
}