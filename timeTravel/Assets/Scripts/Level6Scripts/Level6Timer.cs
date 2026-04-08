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
        public AudioClip urgentSound;
        private AudioSource _audioSource;
        private bool _urgentPlayed = false;

        private float _timeRemaining;
        private bool _timerRunning;
        private bool _timerExpired;

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
            _urgentPlayed = false;

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            if (timerPanel != null)
                timerPanel.SetActive(false);
        }

        private void Update()
        {
            if (!_timerRunning || _timerExpired) return;

            _timeRemaining -= Time.deltaTime;
            UpdateTimerUI();

            // Turn red at 30 seconds
            if (_timeRemaining <= 30f && timerText != null)
                timerText.color = Color.red;

            // Play urgent sound once at 30 seconds
            if (_timeRemaining <= 30f && !_urgentPlayed)
            {
                _urgentPlayed = true;
                PlayUrgentSound();
            }

            if (_timeRemaining <= 0f)
                TimerExpired();
        }

        public void StartTimer()
        {
            _timerRunning = true;
            _timerExpired = false;
            _urgentPlayed = false;
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

            if (_audioSource != null)
                _audioSource.Stop();

            Debug.Log("Level6Timer: Timer stopped!");
        }

        private void PlayUrgentSound()
        {
            if (urgentSound != null && _audioSource != null)
            {
                _audioSource.clip = urgentSound;
                _audioSource.loop = true;
                _audioSource.Play();
                Debug.Log("Level6Timer: Urgent sound playing!");
            }
        }

        private void UpdateTimerUI()
        {
            if (timerText == null) return;

            float t = Mathf.Max(0f, _timeRemaining);
            int minutes = Mathf.FloorToInt(t / 60f);
            int seconds = Mathf.FloorToInt(t % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }

        private void TimerExpired()
        {
            _timerExpired = true;
            _timerRunning = false;

            if (timerPanel != null)
                timerPanel.SetActive(false);

            if (_audioSource != null)
                _audioSource.Stop();

            Debug.Log("Level6Timer: Time expired! Killing player...");

            KillPlayer();
        }

        private void KillPlayer()
        {
            Player.Health ph = null;

            Player.Health[] allHealth = FindObjectsByType<Player.Health>(
                FindObjectsSortMode.None);

            if (allHealth.Length > 0)
            {
                ph = allHealth[0];
                Debug.Log($"Level6Timer: Found Health on {ph.gameObject.name}!");
            }

            if (ph != null)
            {
                ph.Die();
                Debug.Log("Level6Timer: Die() called!");
            }
            else
                Debug.LogWarning("Level6Timer: No Health found anywhere!");
        }
    }
}