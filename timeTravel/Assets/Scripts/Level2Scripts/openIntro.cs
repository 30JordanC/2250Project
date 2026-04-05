using UnityEngine;

public class openIntro : MonoBehaviour
{
    public GameObject introPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show intro UI
            introPanel.SetActive(true);

            // Pause game 
            Time.timeScale = 0f;

            // Unlock cursor so player can click
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameObject.SetActive(false);
            GetComponent<Collider>().enabled = false;
        }
    }
}