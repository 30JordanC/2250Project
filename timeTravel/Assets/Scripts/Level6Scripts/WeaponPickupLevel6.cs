using UnityEngine;

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
                SoundManager.Instance?.PlaySFX(SoundManager.SFX.PickupSword);
                // Switch from ambient to boss music when sword is picked up
                SoundManager.Instance?.CrossfadeMusic(null);
                SoundManager.Instance?.PlayBossMusic();
            }
            else
            {
                Debug.LogWarning("WeaponPickupLevel6: Level6Manager not found.");
            }

            gameObject.SetActive(false);
        }
    }
}