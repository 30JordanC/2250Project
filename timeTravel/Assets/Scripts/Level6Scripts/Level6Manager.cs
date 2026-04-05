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

        [Header("Victory Music")]
        public AudioClip victoryMusic;
        public AudioSource musicSource;

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

            if (musicSource == null)
                musicSource = GetComponent<AudioSource>();

            if (musicSource == null)
                musicSource = gameObject.AddComponent<AudioSource>();

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

            // Stop timer
            if (Level6Timer.Instance != null)
                Level6Timer.Instance.StopTimer();

            // Play victory music
            SoundManager.Instance?.CrossfadeMusic(null);
            SoundManager.Instance?.PlayVictoryMusic();

            // Weaken ghost visually
            WeakenGhost();

            // Give player reward
            GivePlayerReward();

            // Show level complete after delay
            Invoke(nameof(ShowLevelCompleteUI), 1.5f);

            Debug.Log("Level6Manager: Terra collected! Level Complete!");
        }

        public void BossDefeated()
        {
            TerraCollected();
        }

        private void WeakenGhost()
        {
            if (ghostEnemy == null)
                ghostEnemy = GameObject.Find("GhostEnemy");

            if (ghostEnemy != null)
            {
                // Disable ghost AI
                EnemyAI ai = ghostEnemy.GetComponent<EnemyAI>();
                if (ai != null)
                    ai.enabled = false;

                // Disable ghost health
                EnemyHealth health = ghostEnemy.GetComponent<EnemyHealth>();
                if (health != null)
                    health.enabled = false;

                // Make ghost fall and fade
                Rigidbody rb = ghostEnemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                }

                // Destroy ghost after delay
                Destroy(ghostEnemy, 3f);

                Debug.Log("Level6Manager: Ghost lost its powers!");
            }
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
            }
        }

        private void ShowLevelCompleteUI()
        {
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);

                if (levelCompleteText != null)
                    levelCompleteText.text =
                        "THE REALM IS FREED!\n\n" +
                        "You have collected the Terra Artifact!\n" +
                        "The ghost has lost its powers...\n" +
                        "Balance has been restored to this world.\n\n" +
                        "Well done, brave traveller!";
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
            // Invoke(nameof(LoadIntroScene), 6f);
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