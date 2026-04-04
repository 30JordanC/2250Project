using UnityEngine;

namespace Level6Scripts
{
    /// <summary>
    /// Moves the player to the spawn point on scene load.
    /// Works whether player already exists in scene or was just spawned.
    /// Uses a small delay to make sure the player has fully initialized first.
    /// </summary>
    public class Level6SpawnFix : MonoBehaviour
    {
        public Transform spawnPoint;

        private void Start()
        {
            if (spawnPoint == null)
            {
                // Try to find it automatically
                SpawnPoint sp = FindFirstObjectByType<SpawnPoint>();
                if (sp != null)
                    spawnPoint = sp.transform;
                else
                {
                    Debug.LogWarning("Level6SpawnFix: No spawnPoint assigned and none found in scene.");
                    return;
                }
            }

            // Small delay so TestPlayerSpawner (Awake) runs first
            Invoke(nameof(MovePlayerToSpawn), 0.1f);
        }

        private void MovePlayerToSpawn()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogWarning("Level6SpawnFix: No GameObject tagged 'Player' found. Make sure your player prefab has the Player tag.");
                return;
            }

            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Debug.Log($"Level6SpawnFix: Player moved to {spawnPoint.position}");
        }
    }
}