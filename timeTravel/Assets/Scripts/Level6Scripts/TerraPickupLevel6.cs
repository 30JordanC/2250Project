using UnityEngine;

namespace Level6Scripts
{
    public class TerraPickupLevel6 : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        public string interactText = "Collect Terra Artifact!";

        [Header("Require Axe")]
        public bool requireAxe = true;

        [Header("Effects")]
        public AudioClip collectSound;
        public GameObject collectEffect;

        private bool _collected = false;

        public void Interact()
        {
            if (_collected) return;

            if (requireAxe && Level6Manager.Instance != null && !Level6Manager.Instance.hasSword)
            {
                Debug.Log("TerraPickup: You need the Legendary Axe first!");
                return;
            }

            Collect();
        }

        public bool CanInteract()
        {
            if (requireAxe && Level6Manager.Instance != null && !Level6Manager.Instance.hasSword)
                return false;
            return !_collected;
        }

        public string GetInteractText()
        {
            if (requireAxe && Level6Manager.Instance != null && !Level6Manager.Instance.hasSword)
                return "Find the Legendary Axe first!";
            return interactText;
        }

        private void Collect()
        {
            _collected = true;

            // Play sound
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // Spawn effect
            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            // Play pickup sound
            SoundManager.Instance?.PlaySFX(SoundManager.SFX.PickupSword);

            Debug.Log("TerraPickup: Terra Artifact collected! Level Complete!");

            // Complete the level
            if (Level6Manager.Instance != null)
                Level6Manager.Instance.TerraCollected();

            gameObject.SetActive(false);
        }
    }
}