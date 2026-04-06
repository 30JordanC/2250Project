using UnityEngine;
using Player;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit kill zone!");

            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(99999f); // instant kill
            }
        }
    }
}