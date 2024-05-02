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
    public float ladderDetectionRange = 100f;

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
        nearestLadder = FindNearestLadder();

        if (nearestLadder != null)
        {
            Debug.Log($"{gameObject.name}: Ladder detected at {nearestLadder.position}, moving to ladder.");
            MoveToLadder(nearestLadder);
        }
        else
        {
            bool enemyInSeeingRange = Physics.CheckSphere(transform.position, seeingRange, whatIsEnemy);
            bool enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

            if (enemyInSeeingRange && !enemyInAttackRange)
            {
                Debug.Log($"{gameObject.name}: Enemy detected in seeing range, moving towards the enemy.");
                MoveTowardsTarget();
            }
            else if (enemyInAttackRange)
            {
                Debug.Log($"{gameObject.name}: Enemy detected in attack range, engaging.");
                AttackEnemy();
            }
            else
            {
                Debug.Log($"{gameObject.name}: No ladder or enemy in range.");
            }
        }

        // Check if the agent is on an off-mesh link
        if (agent.isOnOffMeshLink)
        {
            Debug.Log($"{gameObject.name}: Agent on Off-Mesh Link.");
            agent.ActivateCurrentOffMeshLink(true);
            agent.CompleteOffMeshLink();
        }
        else
        {
            Debug.Log($"{gameObject.name}: Agent not on Off-Mesh Link.");
        }

        // Debugging the path status
        if (agent.hasPath)
        {
            Debug.Log($"{gameObject.name}: Has path to destination.");
        }
        else
        {
            Debug.Log($"{gameObject.name}: No path to destination.");
        }
    }

    private void MoveTowardsTarget()
    {
        FindClosestEnemy();
        if (targetEnemy != null)
        {
            Debug.Log($"{gameObject.name}: Moving towards enemy at {targetEnemy.position}");
            agent.SetDestination(targetEnemy.position);
        }
        else
        {
            Debug.Log($"{gameObject.name}: No enemies found.");
        }
    }

    private void AttackEnemy()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked && targetEnemy != null)
        {
            Debug.Log($"{gameObject.name}: Attacking enemy at {targetEnemy.position}");
            friendlyGun.Fire();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        Debug.Log($"{gameObject.name}: Ready to attack again.");
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

    private Transform FindNearestLadder()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ladderDetectionRange);
        Transform nearestLadder = null;
        float nearestDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"{gameObject.name}: Checking collider {hitCollider.gameObject.name}");

            if (hitCollider.CompareTag("Ladder"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                Debug.Log($"{gameObject.name}: Ladder detected at distance {distance}");
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestLadder = hitCollider.transform;
                }
            }
        }

        if (nearestLadder != null)
        {
            Debug.Log($"{gameObject.name}: Nearest ladder found at {nearestLadder.position}");
        }
        else
        {
            Debug.Log($"{gameObject.name}: No ladders found within detection range.");
        }

        return nearestLadder;
    }

    private void MoveToLadder(Transform ladder)
    {
        Debug.Log($"{gameObject.name}: Moving to ladder at {ladder.position}");
        agent.SetDestination(ladder.position);
    }
}
