using UnityEngine;
using UnityEngine.UI;
using Player;

namespace Level6Scripts
{
    public class TimerBomb : MonoBehaviour
    {
        [Header("Settings")]
        public float timeLimit = 120f;

        [Header("UI")]
        public Text timerText;
        public GameObject timerPanel;

        private float _timeRemaining;
        private bool _timerRunning;
        private bool _timerExpired;

        public static TimerBomb Instance;

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
            _timeRemaining = timeLimit;

            if (timerPanel != null)
                timerPanel.SetActive(true);

            Debug.Log("TimerBomb: Timer started!");
        }

        public void StopTimer()
        {
            _timerRunning = false;

            if (timerPanel != null)
                timerPanel.SetActive(false);

            Debug.Log("TimerBomb: Timer stopped!");
        }

        private void UpdateTimerUI()
        {
            if (timerText == null) return;

            int minutes = Mathf.FloorToInt(_timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60f);
            timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
        }

        private void TimerExpired()
        {
            _timerExpired = true;
            _timerRunning = false;

            Debug.Log("TimerBomb: Time is up! Player dies!");

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null && SceneTransitionManager.Instance != null)
                playerObj = SceneTransitionManager.Instance.playerRoot;

            if (playerObj != null)
            {
                Health ph = playerObj.GetComponentInChildren<Health>()
                             ?? playerObj.GetComponent<Health>();
                if (ph != null)
                    ph.TakeDamage(9999f);
            }

            if (timerPanel != null)
                timerPanel.SetActive(false);
        }
    }
}