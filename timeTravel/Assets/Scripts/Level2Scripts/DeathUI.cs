using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level2Scripts
{
    public class DeathUI : MonoBehaviour
    {
        public GameObject deathScreen;
        public GameObject player;
        public GameObject openIntro;

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

            // Reset player health before respawning
            ResetPlayerHealth();

            if (RespawnManager.instance != null && player != null)
                RespawnManager.instance.Respawn(player);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;

            // Reset player health before restarting
            ResetPlayerHealth();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void ResetPlayerHealth()
        {
            // Try assigned player first
            GameObject playerObj = player;

            // Fallback — find by tag
            if (playerObj == null)
                playerObj = GameObject.FindGameObjectWithTag("Player");

            // Fallback — find via SceneTransitionManager
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
            }
        }
    }
}