using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform enemyTrench; // Target enemy trench location
    public LayerMask whatIsGround, whatIsEnemy;
    
    // Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    [SerializeField] private FriendlyGunScript friendlyGun;
    
    // States
    public float sightRange, attackRange;
    private bool enemyInSightRange, enemyInAttackRange;

    private void Awake()
    {
        enemyTrench = GameObject.Find("EnemyTrench").transform;
        agent = GetComponent<NavMeshAgent>();
        friendlyGun = GetComponentInChildren<FriendlyGunScript>();
    }

    void Update()
    {
        // Check for sight and attack range
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

        if (!enemyInSightRange && !enemyInAttackRange)
        {
            MoveToEnemyTrench();
        }
        
        if (enemyInSightRange && !enemyInAttackRange)
        {
            ChaseEnemy();
        }
        
        if (enemyInSightRange && enemyInAttackRange)
        {
            AttackEnemy();
        }
    }

    private void MoveToEnemyTrench()
    {
        agent.SetDestination(enemyTrench.position);
    }
    
    private void ChaseEnemy()
    {
        MoveToEnemyTrench();
    }
    
    private void AttackEnemy()
    {
        Transform closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            // Stop the agent
            agent.SetDestination(transform.position);
            
            // Rotate towards the closest enemy
            Vector3 direction = (closestEnemy.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            
            if (!alreadyAttacked)
            {
                friendlyGun.Fire();
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }
    
    private Transform FindClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        
        foreach (Collider enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        
        return closestEnemy;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
