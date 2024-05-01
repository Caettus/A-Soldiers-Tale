using UnityEngine;
using UnityEngine.AI;

public class FriendlyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform targetEnemy;
    public LayerMask whatIsGround, whatIsEnemy;

    // Ranges
    public float seeingRange = 100f;
    public float attackRange = 10f;
    public float ladderDetectionRange = 5f; // Adjusted for more accurate proximity

    // Attacking
    private bool alreadyAttacked;
    [SerializeField] private FriendlyGunScript friendlyGun;
    public float timeBetweenAttacks;

    private Transform nearestLadder; // Store the nearest ladder for use

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        friendlyGun = GetComponentInChildren<FriendlyGunScript>();
        Debug.Log($"{gameObject.name}: Friendly AI Initialized");
    }

    private void Update()
    {
        Debug.Log($"{gameObject.name}: Update tick at position {transform.position}");
        nearestLadder = FindNearestLadder();
        if (nearestLadder != null)
        {
            Debug.Log($"{gameObject.name}: Ladder found at {nearestLadder.position}, attempting to move to ladder.");
            MoveToLadder(nearestLadder);
        }
        else
        {
            Debug.Log($"{gameObject.name}: No ladder found within range.");
            CheckAndEngageEnemy();
        }

        // Check if the agent is on an Off-Mesh Link and trying to traverse it
        if (agent.isOnOffMeshLink)
        {
            Debug.Log($"{gameObject.name}: Agent on Off-Mesh Link, attempting to traverse.");
            agent.CompleteOffMeshLink();
        }
    }

    private void CheckAndEngageEnemy()
    {
        bool enemyInSeeingRange = Physics.CheckSphere(transform.position, seeingRange, whatIsEnemy);
        bool enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

        if (enemyInSeeingRange && !enemyInAttackRange)
        {
            MoveTowardsTarget();
        }
        else if (enemyInAttackRange)
        {
            AttackEnemy();
        }
    }

    private void MoveTowardsTarget()
    {
        FindClosestEnemy();
        if (targetEnemy != null)
        {
            Debug.Log($"{gameObject.name}: Enemy detected at {targetEnemy.position}, moving towards.");
            agent.SetDestination(targetEnemy.position);
        }
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
            Debug.Log($"{gameObject.name}: Closest enemy found at {targetEnemy.position}");
        }
        else
        {
            targetEnemy = null;
            Debug.Log($"{gameObject.name}: No enemies found.");
        }
    }

    private void AttackEnemy()
    {
        if (!alreadyAttacked && targetEnemy != null)
        {
            Debug.Log($"{gameObject.name}: Attacking enemy at {targetEnemy.position}");
            // Your attack logic
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        Debug.Log($"{gameObject.name}: Ready to attack again.");
    }

    private Transform FindNearestLadder()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ladderDetectionRange);
        Transform nearestLadder = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ladder"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestLadder = hitCollider.transform;
                    Debug.Log($"{gameObject.name}: Nearest ladder detected at {nearestLadder.position} with distance {distance}");
                }
            }
        }

        if (nearestLadder == null)
        {
            Debug.Log($"{gameObject.name}: No ladder detected within detection range.");
        }

        return nearestLadder;
    }

    private void MoveToLadder(Transform ladder)
    {
        Debug.Log($"{gameObject.name}: Moving to ladder at {ladder.position}");
        agent.SetDestination(ladder.position);
    }
}
