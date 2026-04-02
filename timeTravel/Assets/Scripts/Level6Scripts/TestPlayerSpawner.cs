using UnityEngine;

namespace Level6Scripts
{
    /// <summary>
    /// EDITOR TESTING ONLY.
    /// Drag your player prefab in here. When you play Level 6 directly
    /// without going through character selection, this spawns the player
    /// at the scene spawn point automatically.
    /// Remove this script or untick it before doing a final build.
    /// </summary>
    public class TestPlayerSpawner : MonoBehaviour
    {
        [Header("Your player prefab")]
        public GameObject playerPrefab;

        [Header("Where to spawn (leave empty to use SpawnPoint in scene)")]
        public Transform spawnOverride;

        private void Awake()
        {
            // If a real player already exists (came from character selection),
            // don't spawn a second one
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                Debug.Log("TestPlayerSpawner: Real player already in scene. Skipping test spawn.");
                Destroy(gameObject);
                return;
            }

            if (playerPrefab == null)
            {
                Debug.LogError("TestPlayerSpawner: playerPrefab not assigned! Drag your player prefab into the Inspector.");
                return;
            }

            // Find spawn position
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            if (spawnOverride != null)
            {
                spawnPos = spawnOverride.position;
                spawnRot = spawnOverride.rotation;
            }
            else
            {
                SpawnPoint sp = FindFirstObjectByType<SpawnPoint>();
                if (sp != null)
                {
                    spawnPos = sp.transform.position;
                    spawnRot = sp.transform.rotation;
                }
                else
                {
                    Debug.LogWarning("TestPlayerSpawner: No SpawnPoint found. Spawning at origin.");
                }
            }

            GameObject player = Instantiate(playerPrefab, spawnPos, spawnRot);

            // Make sure it has the Player tag
            player.tag = "Player";

            Debug.Log($"TestPlayerSpawner: Player spawned at {spawnPos}");
        }
    }
}