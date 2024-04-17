using UnityEngine;

public class Friendly : MonoBehaviour
{
    [SerializeField] float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log(health);
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Friendly died!");
        Destroy(gameObject);
    }
}