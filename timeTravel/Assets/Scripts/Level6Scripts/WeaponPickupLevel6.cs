using UnityEngine;

// FIX: was "namespace level6Scripts" (lowercase l) while everything else uses
// "namespace Level6Scripts" (uppercase L). Unified to Level6Scripts.
namespace Level6Scripts
{
    public class WeaponPickupLevel6 : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (Level6Manager.Instance != null)
            {
                Level6Manager.Instance.CollectSword();
                Debug.Log("Sword picked up!");
            }
            else
            {
                Debug.LogWarning("WeaponPickupLevel6: Level6Manager not found in scene.");
            }

            gameObject.SetActive(false);
        }
    }
}