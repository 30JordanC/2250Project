using UnityEngine;

namespace Level6Scripts
{
    public class GhostStun : MonoBehaviour
    {
        [Header("Stun Settings")]
        public float stunDuration = 3f;
        public float pushBackForce = 5f;

        [Header("Visual")]
        public Renderer ghostRenderer;
        public Color normalColor = Color.white;
        public Color stunnedColor = Color.cyan;

        private bool _isStunned;
        private Rigidbody _rb;
        private EnemyAI _enemyAI;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _enemyAI = GetComponent<EnemyAI>();

            if (ghostRenderer == null)
                ghostRenderer = GetComponentInChildren<Renderer>();
        }

        public void Stun()
        {
            if (_isStunned) return;

            _isStunned = true;

            // Disable AI while stunned
            if (_enemyAI != null)
                _enemyAI.enabled = false;

            // Push ghost back
            if (_rb != null)
            {
                Vector3 pushDirection = transform.forward * -1f;
                _rb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);
            }

            // Change color to show stunned
            if (ghostRenderer != null)
                ghostRenderer.material.color = stunnedColor;

            Debug.Log("GhostStun: Ghost stunned!");

            // Recover after stun duration
            Invoke(nameof(Recover), stunDuration);
        }

        private void Recover()
        {
            _isStunned = false;

            if (_enemyAI != null)
                _enemyAI.enabled = true;

            if (ghostRenderer != null)
                ghostRenderer.material.color = normalColor;

            Debug.Log("GhostStun: Ghost recovered!");
        }
    }
}