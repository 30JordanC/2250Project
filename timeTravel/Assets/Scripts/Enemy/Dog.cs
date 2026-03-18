using UnityEngine;

public class Dog : Enemy
{
    [Header("Dog Specific")]
    public AudioSource barkSound;
    

    protected override void Update()
    {
        base.Update();
        

        if (currentState == "chasing")
        {
            Bark();
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < 1.5f)
            {
                Attack();
            }
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
        if (currentState == "chasing")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // Dogs are faster than base enemy
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position,
                (speed + 1.5f) * Time.deltaTime
            );
        }
    }

    public override void Attack()
    {
        base.Attack();

        Debug.Log("Dog caught the player!");

        // 🚨 Lose condition (IMPORTANT for your level design)
        GameControl.Instance.CheckLoseCondition();
    }
    public override void DetectPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= detectionRange)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    currentState = "Chasing";
                    return;
                }
            }
        }

        currentState = "Idle";
    }
}