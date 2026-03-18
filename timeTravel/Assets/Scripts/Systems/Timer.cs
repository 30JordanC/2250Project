using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining;
    public bool isRunning;

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            isRunning = false;
            TimeExpired();
        }
    }

    public void TimeExpired()
    {
        Debug.Log("Timer expired!");

        if (GameControl.Instance != null)
        {
            GameControl.Instance.CheckLoseCondition();
        }
    }
}