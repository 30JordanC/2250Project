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
                Debug.Log("PlayerAttackLevel6: Pick up the sword first.");
                return;
            }

            // Play sword swing sound
            SoundManager.Instance?.PlaySFX(SoundManager.SFX.SwordSwing);

            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

            bool hitSomething = false;
            foreach (Collider hit in hits)
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    SoundManager.Instance?.PlaySFXAt(SoundManager.SFX.SwordHit, hit.transform.position);
                    hitSomething = true;
                    Debug.Log($"Hit {hit.name} for {attackDamage} damage.");
                }
            }

            if (!hitSomething)
                Debug.Log("Swung but hit nothing. Is the golem on the Enemy Layer?");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}