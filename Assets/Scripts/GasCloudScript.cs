using UnityEngine;

public class GasCloud : MonoBehaviour
{
    public float lifetime = 30f; 
    public float damageRate = 10f; 

    private void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                
                playerHealth.TakeDamage(damageRate * Time.deltaTime);
                Debug.Log(playerHealth.health);
            }
        }
        else if (other.CompareTag("Friendly"))
        {
            Friendly friendlyHealth = other.GetComponent<Friendly>();
            if (friendlyHealth != null)
            {
                
                friendlyHealth.TakeDamage(damageRate * Time.deltaTime);
                Debug.Log(friendlyHealth.health);
            }
        }
    }
}
