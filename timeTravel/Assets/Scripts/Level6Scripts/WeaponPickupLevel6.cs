using UnityEngine;

namespace Level6Scripts
{
    public class WeaponPickupLevel6 : MonoBehaviour, IInteractable
    {
        [Header("Prompt")]
        public string interactText = "Pick up Legendary Axe";

        [Header("Axe Attachment")]
        public GameObject axeModel; // drag the axe 3D model here
        public Vector3 handPositionOffset = new Vector3(0, 0, 0);
        public Vector3 handRotationOffset = new Vector3(0, 0, 0);

        private bool _collected;

        public void Interact()
        {
            if (_collected) return;
            PickupAxe();
        }

        public bool CanInteract()
        {
            return !_collected;
        }

        public string GetInteractText()
        {
            return interactText;
        }

        private void PickupAxe()
        {
            _collected = true;

            // Find player's right hand bone
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Search through all children for right hand bone
                Transform rightHand = FindBone(player.transform, "mixamorig:RightHand");

                if (rightHand != null && axeModel != null)
                {
                    // Detach axe from pickup and attach to hand
                    axeModel.transform.SetParent(rightHand);
                    axeModel.transform.localPosition = handPositionOffset;
                    axeModel.transform.localRotation = Quaternion.Euler(handRotationOffset);
                    axeModel.transform.localScale = Vector3.one;
                    axeModel.SetActive(true);

                    Debug.Log("Axe attached to right hand!");
                }
                else
                {
                    Debug.LogWarning("WeaponPickup: Right hand bone not found or axeModel not assigned!");
                }

                // Enable attack script on player
                PlayerAttackLevel6 attack = player.GetComponentInChildren<PlayerAttackLevel6>();
                if (attack != null)
                    attack.enabled = true;
            }

            if (Level6Manager.Instance != null)
            {
                Level6Manager.Instance.CollectSword();
                SoundManager.Instance?.PlaySFX(SoundManager.SFX.PickupSword);
                SoundManager.Instance?.CrossfadeMusic(null);
                SoundManager.Instance?.PlayBossMusic();
            }

            // Hide the pickup object but keep axe model alive
            // Only disable collider and other components
            GetComponent<Collider>().enabled = false;
            // Hide everything except axeModel
            foreach (Transform child in transform)
            {
                if (axeModel != null && child.gameObject != axeModel)
                    child.gameObject.SetActive(false);
            }
        }

        private Transform FindBone(Transform parent, string boneName)
        {
            if (parent.name == boneName) return parent;
            foreach (Transform child in parent)
            {
                Transform found = FindBone(child, boneName);
                if (found != null) return found;
            }
            return null;
        }
    }
}