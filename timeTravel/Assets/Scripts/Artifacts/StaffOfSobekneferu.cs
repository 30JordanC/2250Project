using UnityEngine;

public class StaffOfSobekneferu : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnID; 

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            Debug.Log("Staff collected!");
            
            gameObject.SetActive(false);
            
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