using UnityEngine;

public class Dog : Enemy
{
    [Header("Dog Specific")]
    public AudioSource barkSound;

    protected override void Update()
    {
        base.Update();

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

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            (speed + 1.5f) * Time.deltaTime
        );
    }
}