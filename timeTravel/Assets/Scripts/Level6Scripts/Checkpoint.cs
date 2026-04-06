using UnityEngine;

namespace Level6Scripts
{
    public class Checkpoint : MonoBehaviour
    {
        public static Vector3 LastCheckpointPosition;
        private bool _activated;

        [Header("Visual")]
        public Renderer checkpointRenderer;
        public Color activatedColor = Color.green;
        public Color defaultColor = Color.grey;

        private void Start()
        {
            // Set default spawn as first checkpoint
            if (LastCheckpointPosition == Vector3.zero)
                LastCheckpointPosition = transform.position;

            if (checkpointRenderer != null)
                checkpointRenderer.material.color = defaultColor;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_activated) return;

            if (other.CompareTag("Player") ||
                other.name.Contains("Player") ||
                other.name.Contains("PlayerRoot"))
            {
                Activate();
            }
        }

        private void Activate()
        {
            _activated = true;
            LastCheckpointPosition = transform.position;

            if (checkpointRenderer != null)
                checkpointRenderer.material.color = activatedColor;

            Debug.Log($"Checkpoint activated at {transform.position}");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}