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
    }

    public void Respawn(GameObject player)
    {
        if (currentCheckpoint != null)
        {
            player.transform.position = currentCheckpoint.position;
        }
        else
        {
            Debug.Log("No checkpoint set!");
        }
    }
}