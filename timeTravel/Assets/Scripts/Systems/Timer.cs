using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining;
    public bool isRunning = false;

    public TextMeshProUGUI timerText;
    public GameObject timeUpScreen; // 👈 DRAG PANEL HERE

    public float startTime = 300f;

    void Start()
    {
        StartTimer(startTime);
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
            TimeExpired();
        }

        UpdateTimerUI();
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isRunning = true;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeRemaining <= 30f)
        {
            timerText.color = Color.red;
        }
    }

    void TimeExpired()
    {
        Debug.Log("Out of time!");

        // 👇 SHOW YOUR DEATH SCREEN DIRECTLY
        if (timeUpScreen != null)
        {
            timeUpScreen.SetActive(true);
        }

        // 👇 PAUSE GAME
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}