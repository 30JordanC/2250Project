using UnityEngine;

namespace Level6Scripts
{
    public class Level6FallDeath : MonoBehaviour
    {
        public float deathY = 8f;
        private bool _isDying;

        private void Update()
        {
            if (_isDying) return;

            if (transform.position.y < deathY)
            {
                _isDying = true;
                Debug.Log("Level6FallDeath: Fell below death zone!");
                KillThis();
            }
        }

        private void KillThis()
        {
            Player.Health ph = GetComponent<Player.Health>()
                               ?? GetComponentInChildren<Player.Health>()
                               ?? GetComponentInParent<Player.Health>();

            if (ph != null)
            {
                ph.Die();
                Debug.Log("Level6FallDeath: Player killed!");
            }
            else
            {
                Debug.LogWarning("Level6FallDeath: Health not found!");
                _isDying = false;
            }
        }

        public void Reset()
        {
            _isDying = false;
        }
    }
}