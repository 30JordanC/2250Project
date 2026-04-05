using UnityEngine;
using UnityEngine.SceneManagement;

public class StaffOfSobekneferu : MonoBehaviour
{
    public string sceneToLoad;

    private static bool shouldTeleportPlayer = false; // ✅ ADDED

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Staff collected!");

            shouldTeleportPlayer = true; // ✅ ADDED

            gameObject.SetActive(false);

            SceneManager.sceneLoaded += OnSceneLoaded; // ✅ ADDED
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // ✅ ADDED
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!shouldTeleportPlayer) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            GameObject spawn = GameObject.FindGameObjectWithTag("SpawnPoint");

            if (spawn != null)
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero; // stop motion
                    rb.angularVelocity = Vector3.zero;
                    rb.position = spawn.transform.position; // ✅ physics-safe move
                }
                else
                {
                    player.transform.position = spawn.transform.position;
                }

                Debug.Log("Player teleported correctly after scene load");
            }
            else
            {
                Debug.LogWarning("No SpawnPoint found!");
            }
        }

        shouldTeleportPlayer = false;
        SceneManager.sceneLoaded -= OnSceneLoaded; // clean up
    }
}