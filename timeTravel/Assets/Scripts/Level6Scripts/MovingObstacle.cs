using UnityEngine;

namespace Level6Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class MovingObstacle : MonoBehaviour
    {
        [Header("Movement")]
        public float moveDistance = 1.5f;
        public float moveSpeed = 1.5f;

        [Header("Damage")]
        public float damageAmount = 30f;

        private Vector3 _startPos;
        private Rigidbody _rb;
        private float _leftLimit;
        private float _rightLimit;

        private void Start()
        {
            _startPos = transform.position;

            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            _rb.useGravity = false;
            _rb.freezeRotation = true;

            // Set movement limits
            _leftLimit = _startPos.x - moveDistance;
            _rightLimit = _startPos.x + moveDistance;

            // Make sure collider is solid
            BoxCollider col = GetComponent<BoxCollider>();
            col.isTrigger = false;
        }

        private void FixedUpdate()
        {
            // Calculate new position
            float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            Vector3 newPos = new Vector3(
                _startPos.x + offset,
                transform.position.y,
                _startPos.z
            );

            // Move using Rigidbody — respects collisions
            _rb.MovePosition(newPos);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"MovingObstacle: Collision with {collision.gameObject.name}");

            // Damage player
            Player.Health ph = collision.gameObject.GetComponent<Player.Health>()
                               ?? collision.gameObject.GetComponentInParent<Player.Health>()
                               ?? collision.gameObject.GetComponentInChildren<Player.Health>();

            if (ph != null)
            {
                ph.TakeDamage(damageAmount);
                Debug.Log($"MovingObstacle: Player hit for {damageAmount} damage!");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 left = transform.position - transform.right * moveDistance;
            Vector3 right = transform.position + transform.right * moveDistance;
            Gizmos.DrawLine(left, right);
            Gizmos.DrawWireSphere(left, 0.2f);
            Gizmos.DrawWireSphere(right, 0.2f);
        }
    }
}