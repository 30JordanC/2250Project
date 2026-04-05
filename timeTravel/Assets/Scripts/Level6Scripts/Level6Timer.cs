using UnityEngine;
using UnityEngine.UI;

namespace Level6Scripts
{
    public class Level6Timer : MonoBehaviour
    {
        [Header("Settings")]
        public float timeLimit = 180f;

        [Header("UI")]
        public Text timerText;
        public GameObject timerPanel;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip tickSound;
        public AudioClip urgentTickSound;

        private float _timeRemaining;
        private bool _timerRunning;
        private bool _timerExpired;
        private float _lastTickTime;

        public static Level6Timer Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _timeRemaining = timeLimit;
            _timerRunning = false;
            _timerExpired = false;

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            if (timerPanel != null)
                timerPanel.SetActive(false);
        }

        private void Update()
        {
            if (!_timerRunning || _timerExpired) return;

            _timeRemaining -= Time.deltaTime;

            UpdateTimerUI();
            PlayTickSound();

            if (_timeRemaining <= 30f && timerText != null)
                timerText.color = Color.red;

            if (_timeRemaining <= 0f)
                TimerExpired();
        }

        public void StartTimer()
        {
            _timerRunning = true;
            _timerExpired = false;
            _timeRemaining = timeLimit;

            if (timerPanel != null)
                timerPanel.SetActive(true);

            if (timerText != null)
                timerText.color = Color.white;

            Debug.Log("Level6Timer: Timer started!");
        }

        public void StopTimer()
        {
            _timerRunning = false;

            if (timerPanel != null)
                timerPanel.SetActive(false);

            if (audioSource != null)
                audioSource.Stop();

            Debug.Log("Level6Timer: Timer stopped!");
        }

        private void PlayTickSound()
        {
            if (audioSource == null) return;

            float tickInterval = _timeRemaining > 30f ? 1f : 0.5f;

            if (Time.time >= _lastTickTime + tickInterval)
            {
                _lastTickTime = Time.time;

                if (_timeRemaining <= 30f && urgentTickSound != null)
                    audioSource.PlayOneShot(urgentTickSound);
                else if (tickSound != null)
                    audioSource.PlayOneShot(tickSound);
            }
        }

        private void UpdateTimerUI()
        {
            if (timerText == null) return;

            float clampedTime = Mathf.Max(0f, _timeRemaining);
            int minutes = Mathf.FloorToInt(clampedTime / 60f);
            int seconds = Mathf.FloorToInt(clampedTime % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }

        private void TimerExpired()
        {
            _timerExpired = true;
            _timerRunning = false;

            Debug.Log("Level6Timer: Time is up!");

            if (timerPanel != null)
                timerPanel.SetActive(false);

            if (audioSource != null)
                audioSource.Stop();

            KillPlayer();
        }

        private void KillPlayer()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj == null && SceneTransitionManager.Instance != null)
                playerObj = SceneTransitionManager.Instance.playerRoot;

            if (playerObj == null)
                playerObj = GameObject.Find("PlayerRoot");

            if (playerObj != null)
            {
                Debug.Log($"Level6Timer: Found {playerObj.name} killing now!");

                Player.Health ph = playerObj.GetComponent<Player.Health>()
                             ?? playerObj.GetComponentInChildren<Player.Health>()
                             ?? playerObj.GetComponentInParent<Player.Health>();

                if (ph != null)
                {
                    ph.Die();
                    Debug.Log("Level6Timer: Die() called!");
                    return;
                }

                Player.Health[] allHealth = FindObjectsByType<Player.Health>(FindObjectsSortMode.None);
                if (allHealth.Length > 0)
                {
                    allHealth[0].Die();
                    Debug.Log("Level6Timer: Die() via FindObjectsByType!");
                }
            }
            else
            {
                Debug.LogWarning("Level6Timer: Player not found!");
            }
        }
    }
}