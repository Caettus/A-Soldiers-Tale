using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, whatIsFriendly;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    [SerializeField] EnemyGunScript enemyGun;

    // States
    public float sightRange, attackRange;
    private Transform target; // Target can be either player or a "Friendly"

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyGun = GetComponentInChildren<EnemyGunScript>();
    }

    void Update()
    {
        // Find the closest target, either player or Friendly
        target = FindClosestTarget();

        // Determine if the target is in sight or attack range
        bool targetInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer | whatIsFriendly);
        bool targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer | whatIsFriendly);

        if (!targetInSightRange && !targetInAttackRange) Patrolling();
        if (targetInSightRange && !targetInAttackRange) ChaseTarget();
        if (targetInSightRange && targetInAttackRange) AttackTarget();
    }

    // Find the closest target that is either a player or a Friendly
    private Transform FindClosestTarget()
    {
        List<GameObject> potentialTargets = new List<GameObject>();
        potentialTargets.Add(player.gameObject); // Always consider the player

        // Add all "Friendly" objects to potential targets
        foreach (var friendly in GameObject.FindGameObjectsWithTag("Friendly"))
        {
            potentialTargets.Add(friendly);
        }

        // Determine the closest target
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;
        foreach (var potentialTarget in potentialTargets)
        {
            float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
            if (distance < closestDistance)
            {
                closestTarget = potentialTarget.transform;
                closestDistance = distance;
            }
        }

        return closestTarget;
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
        else
        {
            walkPointSet = false;
        }
    }

    private void ChaseTarget()
    {
        agent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            enemyGun.Fire();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
