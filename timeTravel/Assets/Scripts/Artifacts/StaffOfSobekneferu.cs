using UnityEngine;

public class StaffOfSobekneferu : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnID; // ✅ set this in Inspector

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            Debug.Log("Staff collected!");

            // ✅ Hide staff immediately
            gameObject.SetActive(false);

            // ✅ Use SceneTransitionManager
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.LoadScene(sceneToLoad, spawnID);
            }
            else
            {
                Debug.LogError("SceneTransitionManager not found!");
            }
        }
    }
}