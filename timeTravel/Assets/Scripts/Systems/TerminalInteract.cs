using UnityEngine;

namespace Systems
{
    public class TerminalInteract : MonoBehaviour, IInteractable
    {
        public bool menuIsOpen;

        private PlayerReferences playerRefs;
        private TerminalMenuController terminalMenuController;

        void Start()
        {
            RefreshReferences();
            menuIsOpen = false;
        }

        void Update()
        {
            if (playerRefs == null || terminalMenuController == null)
            {
                RefreshReferences();
            }

            if (terminalMenuController != null)
            {
                menuIsOpen = terminalMenuController.gameObject.activeSelf;
            }
        }

        void RefreshReferences()
        {
            playerRefs = FindFirstObjectByType<PlayerReferences>();

            if (playerRefs != null && playerRefs.terminalMenu != null)
            {
                terminalMenuController = playerRefs.terminalMenu.GetComponent<TerminalMenuController>();
            }
        }

        public void Interact()
        {
            if (menuIsOpen) return;

            if (terminalMenuController == null)
            {
                RefreshReferences();
            }

            if (terminalMenuController == null)
            {
                //Debug.LogError("TerminalMenuController not found on persistent terminal menu.");
                return;
            }

            terminalMenuController.OpenMenu();
            menuIsOpen = true;
        }

        public bool CanInteract()
        {
            return !menuIsOpen;
        }

        public string GetInteractText()
        {
            return "Press E to time travel";
        }
    }
}