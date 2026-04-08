using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class Rex : MonoBehaviour
{
    public enum TRexState
    {
        Idle,
        ChasingPlayer,
        InvestigatingRock,
        ReturningHome,
        Attacking
    }

    [Header("References")]
    public Transform player;
    public Transform homePoint;
    public Animator animator;
    public NavMeshAgent agent;

    [Header("Detection")]
    public float detectionRadius = 20f;
    public float killRadius = 2.5f;

    [Header("Rock Investigation")]
    public float rockNoticeRadius = 15f;
    public float investigateWaitTime = 4f;

    [Header("Movement")]
    public float chaseSpeed = 6f;
    public float walkSpeed = 2.5f;
    public float stoppingDistance = 1.5f;
    public float rotationSpeed = 8f;

    [Header("Attack")]
    public float damageDelay = 0.5f;

    [Header("Animation")]
    public string isWalkingBool = "IsWalking";
    public string isRunningBool = "IsRunning";
    public string attackTriggerName = "Attack";

    private TRexState currentState = TRexState.Idle;
    private Vector3 currentInvestigationPoint;
    private bool isBusyInvestigating;
    private bool playerDead;
    private bool isAttacking;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (homePoint == null)
        {
            GameObject tempHome = new GameObject(name + "_HomePoint");
            tempHome.transform.position = transform.position;
            homePoint = tempHome.transform;
        }

        agent.stoppingDistance = stoppingDistance;
        SetState(TRexState.Idle);
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (player == null || playerDead)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius &&
            currentState != TRexState.ChasingPlayer &&
            currentState != TRexState.Attacking)
        {
            SetState(TRexState.ChasingPlayer);
        }

        switch (currentState)
        {
            case TRexState.Idle:
                agent.isStopped = true;
                SetAnimationBools(false, false);
                break;

            case TRexState.ChasingPlayer:
                if (isAttacking)
                    return;

                agent.isStopped = false;
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
                SetAnimationBools(false, true);

                FaceMovementDirection();

                if (distanceToPlayer <= killRadius)
                {
                    KillPlayer();
                }
                break;

            case TRexState.InvestigatingRock:
                if (isAttacking)
                    return;

                agent.isStopped = false;

                bool reachedRock = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
                SetAnimationBools(!reachedRock, false);

                if (!reachedRock)
                {
                    FaceMovementDirection();
                }
                break;

            case TRexState.ReturningHome:
                if (isAttacking)
                    return;

                agent.isStopped = false;
                agent.speed = walkSpeed;
                agent.SetDestination(homePoint.position);

                bool reachedHome = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
                SetAnimationBools(!reachedHome, false);

                if (!reachedHome)
                {
                    FaceMovementDirection();
                }

                if (reachedHome)
                {
                    SetState(TRexState.Idle);
                }
                break;

            case TRexState.Attacking:
                agent.isStopped = true;
                SetAnimationBools(false, false);

                if (player != null)
                {
                    FaceTarget(player.position);
                }
                break;
        }
    }

    private void SetState(TRexState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case TRexState.Idle:
                agent.ResetPath();
                agent.isStopped = true;
                SetAnimationBools(false, false);
                break;

            case TRexState.ChasingPlayer:
                agent.isStopped = false;
                agent.speed = chaseSpeed;
                SetAnimationBools(false, true);
                break;

            case TRexState.InvestigatingRock:
                agent.isStopped = false;
                agent.speed = walkSpeed;
                agent.SetDestination(currentInvestigationPoint);
                SetAnimationBools(true, false);
                break;

            case TRexState.ReturningHome:
                agent.isStopped = false;
                agent.speed = walkSpeed;
                agent.SetDestination(homePoint.position);
                SetAnimationBools(true, false);
                break;

            case TRexState.Attacking:
                agent.ResetPath();
                agent.isStopped = true;
                SetAnimationBools(false, false);

                if (animator != null)
                {
                    animator.SetTrigger(attackTriggerName);
                }
                break;
        }
    }

    public void NotifyRockLanded(Vector3 rockPosition)
    {
        if (playerDead || isAttacking)
            return;

        float distanceToRock = Vector3.Distance(transform.position, rockPosition);

        if (distanceToRock > rockNoticeRadius)
            return;

        float distanceToPlayer = player != null
            ? Vector3.Distance(transform.position, player.position)
            : Mathf.Infinity;

        if (distanceToPlayer <= detectionRadius)
            return;

        currentInvestigationPoint = rockPosition;

        if (!isBusyInvestigating)
        {
            StartCoroutine(InvestigateRockRoutine());
        }
    }

    private IEnumerator InvestigateRockRoutine()
    {
        isBusyInvestigating = true;
        SetState(TRexState.InvestigatingRock);

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                isBusyInvestigating = false;
                SetState(TRexState.ChasingPlayer);
                yield break;
            }

            yield return null;
        }

        SetAnimationBools(false, false);
        yield return new WaitForSeconds(investigateWaitTime);

        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isBusyInvestigating = false;
            SetState(TRexState.ChasingPlayer);
            yield break;
        }

        SetState(TRexState.ReturningHome);
        isBusyInvestigating = false;
    }

    private void KillPlayer()
    {
        if (playerDead || isAttacking)
            return;

        isAttacking = true;
        SetState(TRexState.Attacking);

        Invoke(nameof(DealDamage), damageDelay);
    }

    private void DealDamage()
    {
        if (playerDead || player == null)
            return;

        Health health = player.GetComponent<Health>();

        if (health != null)
        {
            playerDead = true;
            health.TakeDamage(health.currentHealth);
        }
    }

    private void SetAnimationBools(bool walking, bool running)
    {
        if (animator == null)
        {
            Debug.LogWarning(name + ": Animator is NULL");
            return;
        }

        animator.SetBool(isWalkingBool, walking);
        animator.SetBool(isRunningBool, running);
    }

    private void FaceMovementDirection()
    {
        if (agent == null)
            return;

        Vector3 moveDirection = agent.desiredVelocity;
        moveDirection.y = 0f;

        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rockNoticeRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
}