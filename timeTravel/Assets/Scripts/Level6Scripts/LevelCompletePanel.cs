using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level6Scripts
{
    public class LevelCompleteUI : MonoBehaviour
    {
        public void Continue()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SceneManager.LoadScene("IntroScene");
        }
    }
}