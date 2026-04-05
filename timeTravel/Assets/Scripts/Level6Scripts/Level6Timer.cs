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

            if (timerPanel != null)
                timerPanel.SetActive(false);
        }

        private void Update()
        {
            if (!_timerRunning || _timerExpired) return;

            _timeRemaining -= Time.deltaTime;
            UpdateTimerUI();

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

            Debug.Log("Level6Timer: Timer stopped!");
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

            Debug.Log("Level6Timer: Time expired! Killing player...");

            KillPlayer();
        }

        private void KillPlayer()
        {
            // Search every possible way
            Player.Health ph = null;

            // Method 1 — by tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                ph = playerObj.GetComponent<Player.Health>()
                     ?? playerObj.GetComponentInChildren<Player.Health>()
                     ?? playerObj.GetComponentInParent<Player.Health>();
            }

            // Method 2 — SceneTransitionManager
            if (ph == null && SceneTransitionManager.Instance != null
                && SceneTransitionManager.Instance.playerRoot != null)
            {
                GameObject root = SceneTransitionManager.Instance.playerRoot;
                ph = root.GetComponent<Player.Health>()
                     ?? root.GetComponentInChildren<Player.Health>();
            }

            // Method 3 — find by name
            if (ph == null)
            {
                GameObject byName = GameObject.Find("PlayerRoot");
                if (byName != null)
                    ph = byName.GetComponentInChildren<Player.Health>();
            }

            // Method 4 — find all Health in scene
            if (ph == null)
            {
                Player.Health[] all = FindObjectsByType<Player.Health>(
                    FindObjectsSortMode.None);
                if (all.Length > 0)
                    ph = all[0];
            }

            if (ph != null)
            {
                Debug.Log("Level6Timer: Found Health — calling Die()!");
                ph.Die();
            }
            else
                Debug.LogWarning("Level6Timer: Could not find Health anywhere!");
        }
    }
}