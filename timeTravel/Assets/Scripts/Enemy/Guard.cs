using UnityEngine;

public class Guard : Enemy, IDamageable
{

//adding in new take damage 
        public void TakeDamage(int amount)
    {
        health -= amount;
        
        //die if no health remaining
        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
