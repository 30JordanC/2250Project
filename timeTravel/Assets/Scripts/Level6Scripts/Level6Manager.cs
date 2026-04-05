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
        public GameObject ghostEnemy;

        [Header("Level Complete UI")]
        public GameObject levelCompletePanel;
        public Text levelCompleteText;

        [Header("Victory Audio")]
        public AudioSource victoryAudioSource;
        public AudioClip victoryClip;

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
            else
                Debug.LogWarning("Level6Manager: terraObject not assigned!");

            if (levelCompletePanel != null)
                levelCompletePanel.SetActive(false);
            else
                Debug.LogWarning("Level6Manager: levelCompletePanel not assigned!");

            if (victoryAudioSource == null)
                victoryAudioSource = gameObject.AddComponent<AudioSource>();

            SetupPlayer();

            Invoke(nameof(StartChallengeTimer), 2f);
        }

        private void SetupPlayer()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj == null && SceneTransitionManager.Instance != null)
                playerObj = SceneTransitionManager.Instance.playerRoot;

            if (playerObj != null)
            {
                Level6FallDeath fallDeath = playerObj.GetComponent<Level6FallDeath>();
                if (fallDeath == null)
                    fallDeath = playerObj.AddComponent<Level6FallDeath>();

                fallDeath.deathY = 8f;
                fallDeath.Reset();

                Debug.Log("Level6Manager: Player setup complete!");
            }
            else
            {
                Invoke(nameof(SetupPlayer), 0.5f);
                Debug.Log("Level6Manager: Player not found yet, retrying...");
            }
        }

        private void StartChallengeTimer()
        {
            Debug.Log("Level6Manager: Starting timer...");

            if (Level6Timer.Instance != null)
            {
                Level6Timer.Instance.StartTimer();
                Debug.Log("Level6Manager: Timer started!");
            }
            else
                Debug.LogWarning("Level6Manager: Level6Timer not found!");
        }

        public void CollectSword()
        {
            hasSword = true;
            Debug.Log("Level6Manager: Axe collected! Press F to attack ghost!");
        }

        public void TerraCollected()
        {
            if (terraCollected) return;
            terraCollected = true;

            Debug.Log("Level6Manager: Terra collected! Level Complete!");

            // Stop timer
            if (Level6Timer.Instance != null)
                Level6Timer.Instance.StopTimer();

            // Play victory music
            PlayVictoryMusic();

            // Weaken ghost
            WeakenGhost();

            // Give player reward
            GivePlayerReward();

            // Show win screen after delay
            Invoke(nameof(ShowLevelCompleteUI), 1.5f);
        }

        public void BossDefeated()
        {
            TerraCollected();
        }

        private void PlayVictoryMusic()
        {
            SoundManager.Instance?.CrossfadeMusic(null);

            if (victoryClip != null && victoryAudioSource != null)
            {
                victoryAudioSource.clip = victoryClip;
                victoryAudioSource.Play();
                Debug.Log("Level6Manager: Victory music playing!");
            }
            else
                SoundManager.Instance?.PlayVictoryMusic();
        }

        private void WeakenGhost()
        {
            if (ghostEnemy == null)
                ghostEnemy = GameObject.Find("GhostEnemy");

            if (ghostEnemy != null)
            {
                EnemyAI ai = ghostEnemy.GetComponent<EnemyAI>();
                if (ai != null) ai.enabled = false;

                EnemyHealth health = ghostEnemy.GetComponent<EnemyHealth>();
                if (health != null) health.enabled = false;

                Rigidbody rb = ghostEnemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                }

                Destroy(ghostEnemy, 3f);
                Debug.Log("Level6Manager: Ghost lost its powers!");
            }
            else
                Debug.LogWarning("Level6Manager: GhostEnemy not found!");
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

                // Remove fall death after winning
                Level6FallDeath fallDeath = playerObj.GetComponent<Level6FallDeath>();
                if (fallDeath != null)
                    Destroy(fallDeath);
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
                {
                    levelCompleteText.text =
                        "THE REALM IS FREED!\n\n" +
                        "You collected the Terra Artifact!\n" +
                        "The ghost has lost its powers...\n" +
                        "Balance has been restored!\n\n" +
                        "Well done, brave traveller!\n" +
                        "YOU WIN!";
                }
                else
                    Debug.LogWarning("Level6Manager: levelCompleteText not assigned!");
            }
            else
                Debug.LogWarning("Level6Manager: levelCompletePanel not assigned!");

            // Pause game on win
            Time.timeScale = 0f;

            // Show cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            CompleteLevel();
        }

        public void CompleteLevel()
        {
            if (levelComplete) return;
            levelComplete = true;

            SoundManager.Instance?.PlaySFX(SoundManager.SFX.LevelComplete);
            Debug.Log("Level6Manager: Level 6 Complete!");
        }
    }
}