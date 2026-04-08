using UnityEngine;

public class BossPlaceholder : MonoBehaviour
{
    public GameObject promptUI; // drag your BossPromptUI here

    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.B))
        {
            SceneTransitionManager.Instance.LoadScene("IntroScene", "BackToIntroSpawn");
            Debug.Log("Going back to intro scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (promptUI != null)
                promptUI.SetActive(true); // show the UI
            Debug.Log("Player entered boss trigger!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (promptUI != null)
                promptUI.SetActive(false); // hide the UI
            Debug.Log("Player left boss trigger!");
        }
    }
}