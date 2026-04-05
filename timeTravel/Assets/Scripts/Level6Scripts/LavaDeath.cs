using UnityEngine;
using Player;
using Level6Scripts;

public class LavaDeath : MonoBehaviour
{
    public float damagePerSecond = 9999f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"LavaDeath triggered by: {other.gameObject.name}");
    }

    private void OnTriggerStay(Collider other)
    {
        // Keep damaging player every frame while in lava
        Health ph = other.GetComponent<Health>()
                    ?? other.GetComponentInParent<Health>()
                    ?? other.GetComponentInChildren<Health>();
        if (ph != null)
        {
            ph.TakeDamage(damagePerSecond * Time.deltaTime);
            return;
        }

        // Keep damaging enemy every frame while in lava
        EnemyHealth eh = other.GetComponent<EnemyHealth>()
                         ?? other.GetComponentInParent<EnemyHealth>();
        if (eh != null)
        {
            eh.TakeDamage(damagePerSecond * Time.deltaTime);
            return;
        }
    }
}