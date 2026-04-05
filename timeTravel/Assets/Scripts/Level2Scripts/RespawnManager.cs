using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public Transform currentCheckpoint;

    private void Awake()
    {
        instance = this;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
        Debug.Log("Checkpoint set to: " + checkpoint.position); 
    }

    public void Respawn(GameObject player)
    {
        if (currentCheckpoint != null)
        {
            TeleportPlayer(player, currentCheckpoint.position); 
        }
        else
        {
            Debug.Log("No checkpoint set!");
        }
    }
    
    public Vector3 GetRespawnPosition()
    {
        if (currentCheckpoint != null)
        {
            return currentCheckpoint.position;
        }

        Debug.LogWarning("No checkpoint set! Returning zero.");
        return Vector3.zero;
    }
    
    private void TeleportPlayer(GameObject player, Vector3 position)
    {
        Rigidbody rb = player.GetComponentInChildren<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = position; // move physics body
        }
        
        player.transform.position = position;

        Debug.Log("Player teleported to: " + position);
    }
}