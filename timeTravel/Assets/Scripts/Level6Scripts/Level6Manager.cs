using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{
    public class Level6Manager : MonoBehaviour
    {
        public static Level6Manager Instance;

        [Header("State")]
        public bool hasSword;
        public bool terraCollected;
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

            if (terraObject != null)
                terraObject.SetActive(true);

            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(false);

            Invoke(nameof(StartChallengeTimer), 2f);
        }

        private void StartChallengeTimer()
        {
            if (Level6Timer.Instance != null)
                Level6Timer.Instance.StartTimer();
            else
                Debug.LogWarning("Level6Manager: Level6Timer not found!");
        }

        public void CollectSword()
        {
            hasSword = true;
            Debug.Log("Level6Manager: Axe collected! Press F to stun ghost, collect Terra!");
        }

        public void TerraCollected()
        {
            if (terraCollected) return;
            terraCollected = true;

            if (Level6Timer.Instance != null)
                Level6Timer.Instance.StopTimer();

            Debug.Log("Level6Manager: Terra collected! Level Complete!");

            GivePlayerReward();

            Invoke(nameof(ShowLevelCompleteUI), 1f);
        }

        public void BossDefeated()
        {
            TerraCollected();
        }

        private void GivePlayerReward()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj == null && SceneTransitionManager.Instance != null)
                playerObj = SceneTransitionManager.Instance.playerRoot;

            if (playerObj != null)
            {
                PlayerMovement movement = playerObj.GetComponentInChildren<PlayerMovement>();
                if (movement != null)
                {
                    movement.walkSpeed += 1f;
                    movement.sprintSpeed += 2f;
                    Debug.Log("Level6Manager: Player speed boosted!");
                }
                else
                    Debug.LogWarning("Level6Manager: PlayerMovement not found.");
            }
            else
                Debug.LogWarning("Level6Manager: Player not found for reward.");
        }

        private void ShowLevelCompleteUI()
        {
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);

                if (levelCompleteText != null)
                    levelCompleteText.text = "Level Complete!\nTerra Artifact Collected!\nThe realm is saved!";
            }

            CompleteLevel();
        }

        public void CompleteLevel()
        {
            if (levelComplete) return;
            levelComplete = true;

            SoundManager.Instance?.PlaySFX(SoundManager.SFX.LevelComplete);
            Debug.Log("Level6Manager: Level 6 Complete!");

            // Uncomment to load next scene:
            // Invoke(nameof(LoadIntroScene), 4f);
        }

        // private void LoadIntroScene()
        // {
        //     if (SceneTransitionManager.Instance != null)
        //         SceneTransitionManager.Instance.LoadScene("IntroScene", "");
        //     else
        //         UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
        // }
    }
}