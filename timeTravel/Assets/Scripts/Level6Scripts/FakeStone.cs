using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{


    public class FakeStone : MonoBehaviour, IInteractable
    {
        [Header("UI")] public string interactText = "Examine Stone";

        [Header("Fake Message")] public GameObject fakeMessagePanel;
        public Text fakeMessageText;
        public float hideDelay = 2f;

        private bool _examined;

        public void Interact()
        {
            if (_examined) return;
            _examined = true;

            if (fakeMessagePanel != null)
            {
                fakeMessagePanel.SetActive(true);

                if (fakeMessageText != null)
                    fakeMessageText.text = "I am not Terra Artifact! Look for blue-orange glow!";

                Invoke(nameof(HideMessage), hideDelay);
            }

            Debug.Log("FakeStone: Player examined a fake stone!");
        }

        public bool CanInteract() => !_examined;

        public string GetInteractText() => interactText;

        private void HideMessage()
        {
            if (fakeMessagePanel != null)
                fakeMessagePanel.SetActive(false);

            // Reset so player can examine again if they want
            _examined = false;
        }
    }
}