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
        MovePlayerToCheckpoint();

        // Restart timer
        if (Level6Scripts.Level6Timer.Instance != null)
            Level6Scripts.Level6Timer.Instance.StartTimer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("DeathUI: Player respawned at checkpoint!");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        // Reset checkpoint
        Level6Scripts.Checkpoint.LastCheckpointPosition = Vector3.zero;

        ResetPlayerHealth();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("DeathUI: Level restarted!");
    }

    private void MovePlayerToCheckpoint()
    {
        if (Level6Scripts.Checkpoint.LastCheckpointPosition == Vector3.zero)
            return;

        GameObject playerObj = GetPlayer();

        if (playerObj != null)
        {
            // Move player to last checkpoint
            Rigidbody rb = playerObj.GetComponentInChildren<Rigidbody>()
                           ?? playerObj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = Level6Scripts.Checkpoint.LastCheckpointPosition;
                Debug.Log("DeathUI: Player moved to checkpoint!");
            }
            else
            {
                playerObj.transform.position =
                    Level6Scripts.Checkpoint.LastCheckpointPosition;
            }
        }
    }

    private void ResetPlayerHealth()
    {
        GameObject playerObj = GetPlayer();

        if (playerObj != null)
        {
            Player.Health ph = playerObj.GetComponentInChildren<Player.Health>()
                                ?? playerObj.GetComponent<Player.Health>();
            if (ph != null)
            {
                ph.ResetHealth();
                Debug.Log("DeathUI: Player health reset!");
            }

            Level6Scripts.Level6FallDeath fallDeath =
                playerObj.GetComponent<Level6Scripts.Level6FallDeath>();
            if (fallDeath != null)
                fallDeath.Reset();
        }
    }

    private GameObject GetPlayer()
    {
        GameObject playerObj = player;

        if (playerObj == null)
            playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj == null && SceneTransitionManager.Instance != null)
            playerObj = SceneTransitionManager.Instance.playerRoot;

        return playerObj;
    }
}