using UnityEngine;

namespace Level6Scripts
{
    public class TerraPickupLevel6 : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (Level6Manager.Instance != null) return;
            if (!Level6Manager.Instance.bossDead) return;
            
            Level6Manager.Instance.CompleteLevel();
            gameObject.SetActive(false);
            Debug.Log("Level 6 Completed");
        }
    }
}