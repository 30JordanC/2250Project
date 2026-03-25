
    using UnityEngine;

    public class PlayerSelectionApplier : MonoBehaviour
    {
        [Header("Assign your two player objects here")]
        public GameObject malePlayer;

        public GameObject femalePlayer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (CharacterSelectionManager.Instance == null)
            {
                Debug.Log("CharacterSelectionManager not found. Defaulting to male.");
                if (malePlayer != null) malePlayer.SetActive(true);
                if (femalePlayer != null) femalePlayer.SetActive(false);
                return;
            }

            switch (CharacterSelectionManager.Instance.selectedCharacter)
            {
                case CharacterSelectionManager.CharacterType.Male:
                    if (malePlayer != null) malePlayer.SetActive(true);
                    if (femalePlayer != null) femalePlayer.SetActive(false);
                    break;
                case CharacterSelectionManager.CharacterType.Female:
                    if (malePlayer != null) malePlayer.SetActive(false);
                    if (femalePlayer != null) femalePlayer.SetActive(true);
                    break;
                default:
                    if (malePlayer != null) malePlayer.SetActive(true);
                    if (femalePlayer != null) femalePlayer.SetActive(false);
                    break;
            }

        }

        
        
    }
