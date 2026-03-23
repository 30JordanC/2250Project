using UnityEngine;

namespace C_Scripts
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] characterOptions;
        [SerializeField] private int currentIndex;

        public int CurrentIndex => currentIndex;

        private void Start()
        {
            if (characterOptions == null || characterOptions.Length == 0)
            {
                Debug.LogWarning("CharacterSelectManager: No characters assigned in characterOptions!");
                return;
            }

            currentIndex = Mathf.Clamp(currentIndex, 0, characterOptions.Length - 1);
            ShowCharacter(currentIndex);
        }

        public void NextCharacter()
        {
            if (characterOptions == null || characterOptions.Length == 0) return;

            currentIndex = (currentIndex + 1) % characterOptions.Length;
            ShowCharacter(currentIndex);
        }

        public void PreviousCharacter()
        {
            if (characterOptions == null || characterOptions.Length == 0) return;

            currentIndex = (currentIndex - 1 + characterOptions.Length) % characterOptions.Length;
            ShowCharacter(currentIndex);
        }

        private void ShowCharacter(int index)
        {
            if (characterOptions == null || characterOptions.Length == 0) return;

            index = Mathf.Clamp(index, 0, characterOptions.Length - 1);

            for (int i = 0; i < characterOptions.Length; i++)
            {
                if (characterOptions[i] != null)
                {
                    characterOptions[i].SetActive(i == index);
                }
            }
        }
    }
}