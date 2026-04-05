using UnityEngine;

public class Dog : Enemy
{
    [Header("Dog Specific")]
    public AudioSource barkSound;
    public Animator animator;
    protected override void Update()
    {
        base.Update();

        if (animator != null)
        {
            animator.SetBool("isRunning", currentState == EnemyState.Chasing);
        }

        if (currentState == EnemyState.Chasing)
        {
            Bark();
        }

        if (currentState == EnemyState.Attacking)
        {
            Debug.Log("Dog caught the player!");
        }
    }

    public void Bark()
    {
        Debug.Log("Dog barking!");

        if (barkSound != null && !barkSound.isPlaying)
        {
            barkSound.Play();
        }
    }

    
    public override void Move()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        // 🔥 CRITICAL FIX
        direction.y = 0;

        direction = direction.normalized;

        // Rotate
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
        }

        // Move
        transform.position += direction * (speed + 1.5f) * Time.deltaTime;
    }
}