using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level3DeathUI : MonoBehaviour
{
    public GameObject deathPanel;

    void Start() => deathPanel.SetActive(false);

    public void ShowDeathScreen()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}