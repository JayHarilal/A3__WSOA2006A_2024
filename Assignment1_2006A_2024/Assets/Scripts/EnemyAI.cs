using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float attackRadius = 2f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public Transform[] patrolPoints;

    private int currentPatrolIndex;
    private NavMeshAgent agent;
    private enum EnemyState { Idle, Patrolling, Chasing, Attacking }
    private EnemyState currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;
        currentPatrolIndex = 0;
        agent.speed = patrolSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRadius)
                {
                    currentState = EnemyState.Chasing;
                    agent.speed = chaseSpeed;
                }
                break;

            case EnemyState.Chasing:
                ChasePlayer();
                if (distanceToPlayer <= attackRadius)
                {
                    currentState = EnemyState.Attacking;
                }
                else if (distanceToPlayer > detectionRadius)
                {
                    currentState = EnemyState.Patrolling;
                    agent.speed = patrolSpeed;
                }
                break;

            case EnemyState.Attacking:
                AttackPlayer();
                if (distanceToPlayer > attackRadius)
                {
                    currentState = EnemyState.Chasing;
                }
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        // Implement attack logic here
        Debug.Log("Attacking Player!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
