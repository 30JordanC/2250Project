using Player;
using UnityEngine;
using UnityEngine.AI;

public class Velociraptor : MonoBehaviour
{
    public enum RaptorMode
    {
        Idle,
        Patrol
    }

    [Header("Mode")]
    public RaptorMode mode = RaptorMode.Idle;

    [Header("References")]
    public Transform player;
    public Transform eyePoint;
    public Transform pointA;
    public Transform pointB;
    public Animator animator;

    private NavMeshAgent agent;

    [Header("Detection")]
    public float detectionRange = 12f;
    public float detectionRadius = 0.5f;
    public float fieldOfView = 100f;
    public float killRange = 1.5f;
    public float immediateDetectionRadius = 2f;

    [Header("Movement")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 6f;
    public float pointReachedDistance = 0.5f;

    private bool aggroed = false;
    private bool playerDead = false;
    private bool isAttacking = false;
    private Transform currentTargetPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (mode == RaptorMode.Idle)
        {
            agent.speed = chaseSpeed;
            agent.isStopped = true;
        }
        else if (mode == RaptorMode.Patrol)
        {
            agent.speed = patrolSpeed;
            currentTargetPoint = pointA;

            if (currentTargetPoint != null)
            {
                agent.SetDestination(currentTargetPoint.position);
            }
        }
    }

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
        }

        if (player == null || eyePoint == null || playerDead) return;

        if (!aggroed)
        {
            if (CanDetectPlayer())
            {
                StartChase();
            }
            else
            {
                if (mode == RaptorMode.Patrol)
                {
                    Patrol();
                }
                else
                {
                    SetAnimationBools(false, false);
                }

                return;
            }
        }

        ChasePlayer();
    }

    void StartChase()
    {
        aggroed = true;
        agent.isStopped = false;
        agent.speed = chaseSpeed;
    }

    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        if (!agent.pathPending && agent.remainingDistance <= pointReachedDistance)
        {
            currentTargetPoint = currentTargetPoint == pointA ? pointB : pointA;
            agent.SetDestination(currentTargetPoint.position);
        }

        SetAnimationBools(true, false);
    }

    void ChasePlayer()
    {
        if (isAttacking) return;

        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= killRange)
        {
            KillPlayer();
            return;
        }

        SetAnimationBools(false, true);
    }

    bool CanDetectPlayer()
    {
        Stealth stealth = player.GetComponent<Stealth>();
        if (stealth != null && stealth.isHidden)
        {
            return false;
        }

        Vector3 toPlayer = player.position - eyePoint.position;
        float distanceToPlayer = toPlayer.magnitude;

        // Immediate close-range detection
        // If the player is very close, detect them even if FOV/cast would fail
        if (distanceToPlayer <= immediateDetectionRadius)
        {
            return true;
        }

        // Normal detection range check
        if (distanceToPlayer > detectionRange)
            return false;

        // FOV check
        float angle = Vector3.Angle(transform.forward, toPlayer);
        if (angle > fieldOfView * 0.5f)
            return false;

        // SphereCastAll toward player direction
        RaycastHit[] hits = Physics.SphereCastAll(
            eyePoint.position,
            detectionRadius,
            toPlayer.normalized,
            detectionRange
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.root == player.root)
            {
                return true;
            }
        }

        return false;
    }

    void KillPlayer()
    {
        if (playerDead || isAttacking) return;

        isAttacking = true;
        SetAnimationBools(false, false);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Invoke(nameof(DealDamage), 0.5f);
    }

    void DealDamage()
    {
        if (playerDead) return;
        Health health = player.GetComponent<Health>();
        
        if (health != null)
        {
            playerDead = true;
            health.TakeDamage(health.currentHealth);
        }
    }

    void SetAnimationBools(bool walking, bool running)
    {
        if (animator == null)
        {
            Debug.LogWarning(name + ": Animator is NULL");
            return;
        }

        animator.SetBool("IsWalking", walking);
        animator.SetBool("IsRunning", running);
    }

    void OnDrawGizmosSelected()
    {
        if (eyePoint != null)
        {
            // SphereCast visualization (tube)
            Gizmos.color = Color.red;

            Vector3 start = eyePoint.position;
            Vector3 end = eyePoint.position + transform.forward * detectionRange;

            Gizmos.DrawWireSphere(start, detectionRadius);
            Gizmos.DrawWireSphere(end, detectionRadius);

            Vector3 upOffset = transform.up * detectionRadius;
            Vector3 rightOffset = transform.right * detectionRadius;

            Gizmos.DrawLine(start + upOffset, end + upOffset);
            Gizmos.DrawLine(start - upOffset, end - upOffset);
            Gizmos.DrawLine(start + rightOffset, end + rightOffset);
            Gizmos.DrawLine(start - rightOffset, end - rightOffset);

            // FOV lines
            Vector3 left = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up) * transform.forward;
            Vector3 right = Quaternion.AngleAxis(fieldOfView * 0.5f, Vector3.up) * transform.forward;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(eyePoint.position, eyePoint.position + left * detectionRange);
            Gizmos.DrawLine(eyePoint.position, eyePoint.position + right * detectionRange);
        }

        // Immediate detection radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(eyePoint != null ? eyePoint.position : transform.position, immediateDetectionRadius);

        // Kill range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, killRange);

        // Patrol points
        if (mode == RaptorMode.Patrol && pointA != null && pointB != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}