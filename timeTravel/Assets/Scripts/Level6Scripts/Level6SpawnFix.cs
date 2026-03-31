using UnityEngine;

namespace Level6Scripts
{
    public class Level6SpawnFix : MonoBehaviour
    {
        public Transform spawnPoint;

        void Start()
        {
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
        }
    }
}