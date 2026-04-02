using UnityEngine;

namespace Level6Scripts
{
    public class TerraPickupLevel6 : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            // FIX: was "if Instance != null return" which exited when
            // the manager DID exist — meaning CompleteLevel() never ran.
            // Now correctly exits only when manager is MISSING.
            if (Level6Manager.Instance == null)
            {
                Debug.LogWarning("TerraPickupLevel6: Level6Manager not found in scene.");
                return;
            }

            // FIX: was "if Instance != null return" above meaning this line
            // would have caused a null reference crash anyway.
            // Now correctly checks boss is dead before allowing completion.
            if (!Level6Manager.Instance.bossDead)
            {
                Debug.Log("TerraPickupLevel6: Boss is not dead yet. Defeat the golem first.");
                return;
            }

            Level6Manager.Instance.CompleteLevel();
            gameObject.SetActive(false);
            Debug.Log("Level 6 Completed!");
        }
    }
}