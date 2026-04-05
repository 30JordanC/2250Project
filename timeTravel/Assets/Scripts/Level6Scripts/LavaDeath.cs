using UnityEngine;

public class LavaDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"LavaDeath: Triggered by {other.gameObject.name}");
        TryKill(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryKill(other);
    }

    private void TryKill(Collider other)
    {
        if (other.gameObject == gameObject) return;
        if (other.isTrigger) return;

        // Kill player
        Player.Health ph = other.GetComponent<Player.Health>()
                           ?? other.GetComponentInParent<Player.Health>()
                           ?? other.GetComponentInChildren<Player.Health>();

        if (ph != null)
        {
            Debug.Log("LavaDeath: Killing player!");
            ph.Die();
            return;
        }

        // Kill enemy
        Level6Scripts.EnemyHealth eh =
            other.GetComponent<Level6Scripts.EnemyHealth>()
            ?? other.GetComponentInParent<Level6Scripts.EnemyHealth>();

        if (eh != null)
        {
            Debug.Log("LavaDeath: Killing enemy!");
            eh.TakeDamage(9999f);
        }
    }
}