using UnityEngine;

public class FriendlyGunScript : MonoBehaviour
{
    [SerializeField] public float range = 100f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitEffect;
    
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime = 0f;

    public void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Debug.Log("Friendly unit fired the gun!");
            ProcessRaycast();
            PlayMuzzleFlash();
        }
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    void ProcessRaycast()
    {
        RaycastHit hit;
        // Add a random offset to the direction of the raycast for a more realistic aiming error
        Vector3 direction = firePoint.forward + new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), UnityEngine.Random.Range(-0.05f, 0.05f), UnityEngine.Random.Range(-0.05f, 0.05f));
        if (Physics.Raycast(firePoint.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemyHealth = hit.transform.GetComponent<Enemy>();

            // Assuming enemies have a specific tag, such as "Enemy"
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy!");
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log("Enemy took damage!");
                }
            }

            // Optionally, create a visual effect for the hit
            CreateHitImpact(hit);
        }
        else
        {
            Debug.Log("Missed!");
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 1);
    }
}
