using UnityEngine;

namespace Level6Scripts
{
    public class Level6Manager : MonoBehaviour
    {
        public static Level6Manager Instance;

        public bool hasSword;
        public bool bossDead;
        public bool levelComplete;

        public GameObject terraObject;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Start with background ambient music
            SoundManager.Instance?.PlayBackgroundMusic();
        }

        public void CollectSword()
        {
            hasSword = true;
            Debug.Log("Level6Manager: Sword collected! Press F to attack.");
        }

        public void BossDefeated()
        {
            bossDead = true;
            Debug.Log("Level6Manager: Boss defeated! Terra pickup is now active.");

            if (terraObject != null)
                terraObject.SetActive(true);
            else
                Debug.LogWarning("Level6Manager: terraObject not assigned in Inspector.");
        }

        public void CompleteLevel()
        {
            if (levelComplete) return;
            levelComplete = true;

            SoundManager.Instance?.PlaySFX(SoundManager.SFX.LevelComplete);
            Debug.Log("Level6Manager: Level 6 Complete!");

            // Uncomment to load next scene:
            // SceneTransitionManager.Instance?.LoadScene("YourNextScene", "SpawnID");
        }
    }
}