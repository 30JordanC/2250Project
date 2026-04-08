using UnityEngine;
using UnityEngine.SceneManagement;

public class StaffOfSobekneferu : MonoBehaviour
{
    public string sceneToLoad; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Staff collected!");

            gameObject.SetActive(false);

            // Load next scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}