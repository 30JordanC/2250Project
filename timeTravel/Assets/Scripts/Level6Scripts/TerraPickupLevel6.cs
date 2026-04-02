using UnityEngine;

namespace Level6Scripts
{
    public class TerraPickupLevel6 : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (Level6Manager.Instance == null)
            {
                Debug.LogWarning("TerraPickupLevel6: Level6Manager not found.");
                return;
            }

            if (!Level6Manager.Instance.bossDead)
            {
                Debug.Log("TerraPickupLevel6: Boss is not dead yet.");
                return;
            }

            SoundManager.Instance?.PlaySFX(SoundManager.SFX.PickupTerra);
            Level6Manager.Instance.CompleteLevel();
            gameObject.SetActive(false);
            Debug.Log("Level 6 Completed!");
        }
    }
}