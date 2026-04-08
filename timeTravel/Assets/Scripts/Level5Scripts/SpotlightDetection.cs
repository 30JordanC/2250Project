using UnityEngine;

public class SpotlightDetection : MonoBehaviour
{
    public bool playerDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log("🚨 PLAYER DETECTED BY LIGHT!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            Debug.Log("Player left the light.");
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player INSIDE trigger");
        }
    }
}