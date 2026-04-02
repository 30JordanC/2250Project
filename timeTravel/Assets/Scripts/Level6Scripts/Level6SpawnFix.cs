using UnityEngine;

namespace Level6Scripts
{
    public class Level6SpawnFix : MonoBehaviour
    {
        public Transform spawnPoint;

        private void Start()
        {
            // FIX: Added null check on spawnPoint to prevent crash if not assigned
            if (spawnPoint == null)
            {
                Debug.LogWarning("Level6SpawnFix: spawnPoint is not assigned in Inspector.");
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;

                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                Debug.LogWarning("Level6SpawnFix: No GameObject tagged 'Player' found in scene.");
            }
        }
    }
}