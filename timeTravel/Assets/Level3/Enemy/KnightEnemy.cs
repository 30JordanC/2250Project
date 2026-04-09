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
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        if (patrolPoints.Length > 0)
            GoToNext();
    }

    void Update()
    {
        if (patrolPoints.Length == 0) return;
        if (!agent.isOnNavMesh) return; // safety check

        if (waiting)
        {
            waitTimer -= Time.deltaTime;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.SceneManager
                .LoadScene(
                    UnityEngine.SceneManagement
                    .SceneManager.GetActiveScene().name);
        }
    }
}