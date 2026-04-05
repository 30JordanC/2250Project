using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{
    public class FakeStone : MonoBehaviour, IInteractable
    {
        [Header("UI")]
        public string interactText = "Examine Stone";

        [Header("Fake Message")]
        public GameObject fakeMessagePanel;
        public Text fakeMessageText;
        public float hideDelay = 3f;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip fakeStoneAudio;

        private bool _examined;

        private void Start()
        {
            // Auto-add AudioSource if missing
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Interact()
        {
            if (_examined) return;
            _examined = true;

            // Show a message panel
            if (fakeMessagePanel != null)
            {
                fakeMessagePanel.SetActive(true);

                if (fakeMessageText != null)
                    fakeMessageText.text = "I am not Terra Artifact! Look for blue-orange glow!";

                Invoke(nameof(HideMessage), hideDelay);
            }

            // Play audio
            if (fakeStoneAudio != null && audioSource != null)
                audioSource.PlayOneShot(fakeStoneAudio);

            Debug.Log("FakeStone: Player examined a fake stone!");
        }

        public bool CanInteract() => !_examined;

        public string GetInteractText() => interactText;

        private void HideMessage()
        {
            if (fakeMessagePanel != null)
                fakeMessagePanel.SetActive(false);

            // Stop audio when a message hides
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();

            _examined = false;
        }
    }
}