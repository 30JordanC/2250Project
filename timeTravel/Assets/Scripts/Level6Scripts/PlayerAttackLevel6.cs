using UnityEngine;

namespace Level6Scripts
{
    public class PlayerAttackLevel6 : MonoBehaviour
    {
        public float attackRange = 2.5f;
        public float attackDamage = 25f;
        public LayerMask enemyLayer;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                Attack();
        }

        private void Attack()
        {
            if (Level6Manager.Instance == null)
            {
                Debug.LogWarning("PlayerAttackLevel6: Level6Manager not found.");
                return;
            }

            if (!Level6Manager.Instance.hasSword)
            {
                Debug.Log("PlayerAttackLevel6: No sword yet. Pick up the sword first.");
                return;
            }

            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

            if (hits.Length == 0)
                Debug.Log("PlayerAttackLevel6: Swung but hit nothing. Is the golem on the Enemy Layer?");

            foreach (Collider hit in hits)
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    Debug.Log($"PlayerAttackLevel6: Hit {hit.name} for {attackDamage} damage.");
                }
            }
        }

        // Draws the attack range in the Scene view so you can see it
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}