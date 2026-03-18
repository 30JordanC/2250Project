using UnityEngine;

public class Enemy : MonoBehaviour
{

    //properties

    public int health = 100;
    public float detectionRange = 10f;
    public int damage = 10;
    public float speed = 3f;
    public float attackInterval = 2f;
    public Vector3 spawnPosition;

    protected string currentState = "idle";
    protected Transform target;

    //used to make sure there is actually a cooldown between attacks, and not either attacking every frame, or crashing
    protected float lastAttackTime;

    //methods

    //set spawn position at start only
    void Start()
    {
        transform.position = spawnPosition;
    }

    protected virtual void Update()
    {
        DetectPlayer();

        switch (currentState)
        {
            case "chasing":
                Move();
                break;

            case "attacking":
                Attack();
                break;
        }
    }

    public virtual void DetectPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if ( player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= detectionRange)
        {
            target = player.transform;
            currentState = "idle";
        }
    }

    public virtual void Move()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= 2f) // attack range
        {
            currentState = "attacking";
        }
    }

    public virtual void Attack()
    {
        if (target == null) return;

        if (Time.time >= lastAttackTime + attackInterval)
        {
            IDamageable dmg = target.GetComponent<IDamageable>();

            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }
    }

}
