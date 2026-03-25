
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CharacterSelectionManager : MonoBehaviour
    {
        public static CharacterSelectionManager Instance;

        public enum CharacterType
        {
            None,
            Male,
            Female
        }

        public CharacterType selectedCharacter = CharacterType.None;

        [Header("Next Scene")] public string nextSceneName = "IntroScene";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SelectMale()
        {
            selectedCharacter = CharacterType.Male;
            Debug.Log("Male selected");
            SceneManager.LoadScene(nextSceneName);
        }

        public void SelectFemale()
        {
            selectedCharacter = CharacterType.Female;
            Debug.Log("Female selected");
            SceneManager.LoadScene(nextSceneName);
        }

    }
