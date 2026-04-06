using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject player;
    public GameObject openIntro;

    private void Start()
    {
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
            deathScreen.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("DeathUI: Death screen shown!");
    }

    public void Respawn()
    {
        Time.timeScale = 1f;

        if (deathScreen != null)
            deathScreen.SetActive(false);

        if (openIntro != null)
            openIntro.SetActive(false);

        ResetPlayerHealth();

        if (RespawnManager.instance != null && player != null)
            RespawnManager.instance.Respawn(player);

        // Restart timer
        if (Level6Scripts.Level6Timer.Instance != null)
            Level6Scripts.Level6Timer.Instance.StartTimer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("DeathUI: Player respawned!");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        ResetPlayerHealth();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("DeathUI: Level restarted!");
    }

    private void ResetPlayerHealth()
    {
        GameObject playerObj = player;

        if (playerObj == null)
            playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null && SceneTransitionManager.Instance != null)
            playerObj = SceneTransitionManager.Instance.playerRoot;

        if (playerObj != null)
        {
            Player.Health ph = playerObj.GetComponentInChildren<Player.Health>()
                                ?? playerObj.GetComponent<Player.Health>();
            if (ph != null)
            {
                ph.ResetHealth();
                Debug.Log("DeathUI: Player health reset!");
            }

            // Reset fall death
            Level6Scripts.Level6FallDeath fallDeath =
                playerObj.GetComponent<Level6Scripts.Level6FallDeath>();
            if (fallDeath != null)
                fallDeath.Reset();
        }
    }
}