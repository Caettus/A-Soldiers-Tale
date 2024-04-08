using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float health = 100f;

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
        Debug.Log("noob");
    }
}