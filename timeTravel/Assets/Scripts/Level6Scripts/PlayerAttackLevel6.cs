using UnityEngine;
using Level6Scripts;

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
            {
                Attack();
            }
        }

        private void Attack()
        {
            if (Level6Manager.Instance == null) return;
            if (!Level6Manager.Instance.hasSword) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

            foreach (Collider hit in hits)
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }
    }
}