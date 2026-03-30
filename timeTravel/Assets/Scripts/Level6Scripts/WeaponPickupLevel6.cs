using Level6Scripts;
using UnityEngine;

namespace level6Scripts
{
    
    public class WeaponPickupLevel6 : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (Level6Manager.Instance != null)
            {
                Level6Manager.Instance.CollectSword();
            }

            gameObject.SetActive(false);
        }
    }
}