using UnityEngine;

namespace Level6Scripts
{
    public class Level6DeathTrigger : MonoBehaviour
    {
        private Player.Health _health;
        private GameObject _deathScreen;
        private bool _isDead;

        private void Start()
        {
            InvokeRepeating(nameof(FindComponents), 0f, 0.5f);
        }

        private void FindComponents()
        {
            if (_health == null)
                _health = FindFirstObjectByType<Player.Health>();

            if (_deathScreen == null)
                _deathScreen = GameObject.Find("DeathScreen");

            if (_health != null && _deathScreen != null)
            {
                CancelInvoke(nameof(FindComponents));
                Debug.Log("Level6DeathTrigger: All components found!");
            }
        }

        private void Update()
        {
            if (_health == null || _isDead) return;

            if (_health.currentHealth <= 0f)
            {
                _isDead = true;
                ShowDeathScreen();
            }
        }

        private void ShowDeathScreen()
        {
            if (_deathScreen != null)
            {
                _deathScreen.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("Level6DeathTrigger: Death screen shown!");
            }
            else
                Debug.LogWarning("Level6DeathTrigger: DeathScreen not found!");
        }

        public void Reset()
        {
            _isDead = false;
            if (_deathScreen != null)
                _deathScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}