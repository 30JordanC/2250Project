using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{
    public class Level6Manager : MonoBehaviour
    {
        public static Level6Manager Instance;

        [Header("State")]
        public bool hasSword;
        public bool bossDead;
        public bool levelComplete;

        [Header("Objects")]
        public GameObject terraObject;

        [Header("Level Complete UI")]
        public GameObject levelCompletePanel;
        public Text levelCompleteText;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            SoundManager.Instance?.PlayBackgroundMusic();

            // Hide terra object at start
            if (terraObject != null)
                terraObject.SetActive(false);

            // Hide level complete panel at start
            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(false);
        }

        public void CollectSword()
        {
            hasSword = true;
            Debug.Log("Level6Manager: Axe collected! Press F to attack.");
        }

        public void BossDefeated()
        {
            if (bossDead) return;
            bossDead = true;

            Debug.Log("Level6Manager: Ghost defeated! Terra pickup is now active.");

            // Show terra stone
            if (terraObject != null)
                terraObject.SetActive(true);
            else
                Debug.LogWarning("Level6Manager: terraObject not assigned in Inspector.");

            // Play victory music
            SoundManager.Instance?.CrossfadeMusic(null);
            SoundManager.Instance?.PlayVictoryMusic();

            // Give player speed boost
            GivePlayerReward();

            // Show level complete UI after delay
            Invoke(nameof(ShowLevelCompleteUI), 2f);
        }

        private void GivePlayerReward()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj == null)
            {
                if (SceneTransitionManager.Instance != null &&
                    SceneTransitionManager.Instance.playerRoot != null)
                    playerObj = SceneTransitionManager.Instance.playerRoot;
            }

            if (playerObj != null)
            {
                PlayerMovement movement = playerObj.GetComponentInChildren<PlayerMovement>();
                if (movement != null)
                {
                    movement.walkSpeed += 1f;
                    movement.sprintSpeed += 2f;
                    Debug.Log("Level6Manager: Player speed boosted as reward!");
                }
                else
                {
                    Debug.LogWarning("Level6Manager: PlayerMovement not found on player.");
                }
            }
            else
            {
                Debug.LogWarning("Level6Manager: Player not found for reward.");
            }
        }

        private void ShowLevelCompleteUI()
        {
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);

                if (levelCompleteText != null)
                    levelCompleteText.text = "Level Complete!\nThe ghost has been defeated!\nTerra Stone revealed!";
            }

            CompleteLevel();
        }

        public void CompleteLevel()
        {
            if (levelComplete) return;
            levelComplete = true;

            SoundManager.Instance?.PlaySFX(SoundManager.SFX.LevelComplete);
            Debug.Log("Level6Manager: Level 6 Complete!");

            // Removed auto scene load — player stays in level
            // Uncomment below when ready to go back to intro:
            // Invoke(nameof(LoadIntroScene), 4f);
        }

        // Uncomment when ready to use:
        // private void LoadIntroScene()
        // {
        //     if (SceneTransitionManager.Instance != null)
        //         SceneTransitionManager.Instance.LoadScene("IntroScene", "");
        //     else
        //         UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
        // }
    }
}