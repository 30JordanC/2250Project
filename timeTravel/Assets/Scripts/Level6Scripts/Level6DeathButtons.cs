using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level6Scripts
{
    public class Level6DeathButtons : MonoBehaviour
    {
        public void Respawn()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Hide death screen
            gameObject.SetActive(false);

            // Reset player health
            Player.Health ph = FindFirstObjectByType<Player.Health>();
            if (ph != null)
            {
                ph.currentHealth = ph.maxHealth;
                Debug.Log("Level6DeathButtons: Health reset!");
            }

            // Reset death trigger
            Level6Scripts.Level6DeathTrigger dt =
                FindFirstObjectByType<Level6Scripts.Level6DeathTrigger>();
            if (dt != null) dt.Reset();

            // Reset fall death
            Level6Scripts.Level6FallDeath fd =
                FindFirstObjectByType<Level6Scripts.Level6FallDeath>();
            if (fd != null) fd.Reset();

            // Restart timer
            if (Level6Scripts.Level6Timer.Instance != null)
                Level6Scripts.Level6Timer.Instance.StartTimer();

            // Move player to spawnpoint
            GameObject spawnPoint = GameObject.Find("Spawnpoint");
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj == null && SceneTransitionManager.Instance != null)
                playerObj = SceneTransitionManager.Instance.playerRoot;

            if (playerObj != null && spawnPoint != null)
            {
                Rigidbody rb = playerObj.GetComponentInChildren<Rigidbody>()
                               ?? playerObj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.position = spawnPoint.transform.position;
                }
                else
                    playerObj.transform.position = spawnPoint.transform.position;

                Debug.Log("Level6DeathButtons: Player moved to spawnpoint!");
            }

            Debug.Log("Level6DeathButtons: Respawned!");
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Level6DeathButtons: Restarting level!");
        }
    }
}