using UnityEngine;
using UnityEngine.AI;

public class FriendlyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform targetEnemy;
    public LayerMask whatIsGround, whatIsEnemy;

    // Ranges
    public float seeingRange = 100f; // Distance at which enemies can be detected
    public float attackRange = 10f; // Distance within which the AI can attack

    // Attacking
    private bool alreadyAttacked;
    [SerializeField] private FriendlyGunScript friendlyGun;
    public float timeBetweenAttacks;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        friendlyGun = GetComponentInChildren<FriendlyGunScript>();
    }

    private void Update()
    {
        // Check if any enemy is within seeing range
        bool enemyInSeeingRange = Physics.CheckSphere(transform.position, seeingRange, whatIsEnemy);
        bool enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

        if (enemyInSeeingRange && !enemyInAttackRange)
        {
            // Move towards the target enemy
            MoveTowardsTarget();
        }

        if (enemyInAttackRange)
        {
            // Engage the enemy
            AttackEnemy();
        }

        // If no enemy is within seeing range, consider patrolling or holding position
    }

    private void MoveTowardsTarget()
    {
        FindClosestEnemy(); // Update targetEnemy to the closest enemy
        if (targetEnemy != null)
        {
            agent.SetDestination(targetEnemy.position);
        }
    }

    private void AttackEnemy()
    {
        // Stop moving to attack
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            // Attack logic here
            friendlyGun.Fire();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            targetEnemy = closestEnemy.transform;
        }
        else
        {
            targetEnemy = null;
        }
    }
}
