using UnityEngine;
using UnityEngine.AI;

public class KnightEnemy : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 3f;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private int currentPoint = 0;
    private float waitTimer = 0f;
    private bool waiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //navmesh
        agent.speed = moveSpeed;
        if (patrolPoints.Length > 0)
            GoToNext();
    }
//waypoints used for the guards
    void Update()
    {
        if (patrolPoints.Length == 0) return; //checks if the guard has reached their current point
        if (!agent.isOnNavMesh) return;

        if (waiting)
        {
            waitTimer -= Time.deltaTime; //waits
            if (waitTimer <= 0f)
            {
                waiting = false;
                GoToNext();
            }
            return;
        }

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            waiting = true;
            waitTimer = waitTime;
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }

    void GoToNext()
    {
        if (!agent.isOnNavMesh) return;
        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    void OnTriggerEnter(Collider other) //player enters trigger and dies 
    {
        if (!other.CompareTag("Player")) return;
        
        Level3DeathUI deathUI = FindFirstObjectByType<Level3DeathUI>();
        if (deathUI != null)
            deathUI.ShowDeathScreen();
    }
}