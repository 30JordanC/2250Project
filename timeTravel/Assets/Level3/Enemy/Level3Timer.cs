using UnityEngine;
using TMPro;

public class Level3Timer : MonoBehaviour
{
    [Header("Settings")]
    public float timeLimit = 120f; // 2 minutes

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text headerText;

    private float timeRemaining;
    private bool timerRunning = true;
    private bool expired = false;

    void Start()
    {
        timeRemaining = timeLimit;
        if (headerText != null)
            headerText.text = "Find the runes before the Astrolabe explodes!";
    }

    void Update() 
    {
        if (!timerRunning || expired) return;

        // Check if puzzle already complete
        if (RunePuzzle.activatedCount >= RunePuzzle.totalRunes)
        {
            timerRunning = false;
            if (timerText != null) timerText.text = "Astrolabe Stabilized!";
            return;
        }

        timeRemaining -= Time.deltaTime;

        // Update UI
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        if (timerText != null)
        {
            timerText.text = $"{minutes:00}:{seconds:00}";
            // Turn red when under 30 seconds for visual urgency
            timerText.color = timeRemaining <= 30f ? Color.red : Color.white;
        }

        if (timeRemaining <= 0f)
        {
            expired = true;
            timerRunning = false;
            TimerExpired();
        }
    }

    void TimerExpired()
    {
        Debug.Log("Time up! Astrolabe exploded!");
        // Reload level
        UnityEngine.SceneManagement.SceneManager //to restart
            .LoadScene(
                UnityEngine.SceneManagement
                .SceneManager.GetActiveScene().name);
    }

    public void StopTimer() => timerRunning = false;
}