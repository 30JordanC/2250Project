using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Stats")]
    public int health = 100;
    public float detectionRange = 5f;
    public int damage = 10;
    public float speed = 2f;
    public float attackInterval = 2f;

    protected Transform player;
    protected float lastAttackTime;
    protected string mode = "Idle";

    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        DetectPlayer();
        Move();
    }

    // 👁 Detect Player
    public virtual void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            mode = "Chasing";
        }
        else
        {
            mode = "Idle";
        }
    }

    // 🚶 Movement (basic)
    public virtual void Move()
    {
        if (mode == "Chasing")
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }

    // ⚔ Attack
    public virtual void Attack()
    {
        if (Time.time - lastAttackTime >= attackInterval)
        {
            lastAttackTime = Time.time;

            Debug.Log("Enemy attacks player!");

            // You can later hook into Player health here
        }
    }
}