using Player;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Chasing, Attacking }

    [Header("Stats")]
    public int health = 100;
    public float detectionRange = 10f;
    public int damage = 10;
    public float speed = 3f;
    public float attackRange = 2f;
    public float attackInterval = 2f;

    protected EnemyState currentState = EnemyState.Idle;
    protected Transform target;
    protected float lastAttackTime;

    protected virtual void Update()
    {
        UpdateState();

        switch (currentState)
        {
            case EnemyState.Chasing:
                Move();
                break;

            case EnemyState.Attacking:
                Attack();
                break;
        }
    }

    void UpdateState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            currentState = EnemyState.Idle;
            return;
        }

        target = player.transform;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        else if (distance <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Idle;
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
    }

    public virtual void Attack()
    {
        if (target == null) return;

        if (Time.time >= lastAttackTime + attackInterval)
        {
            Debug.Log("ATTACK CALLED");

            Health health = target.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage((float)damage);
                Debug.Log("Dealt damage to player!");
            }

            lastAttackTime = Time.time;
        }
    }
}