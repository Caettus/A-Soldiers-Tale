using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
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
    public bool playerInSightRange, playerInAttackRange;

    //Awake wordt gecalled wanneer de script wordt geload
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyGun = GetComponentInChildren<EnemyGunScript>();
    }
    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
        }
        
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
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
            // Debug.Log("Walkpoint reached");
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
            // Debug.Log("Found a valid walk point at distance: " + Vector3.Distance(transform.position, walkPoint));
        }
        else
        {
            walkPointSet = false;
            // Debug.Log("Failed to find a valid walk point");
        }
    }
    
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);
        
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
