using System;
using System.Collections;
using UnityEngine;

public class EnemyGunScript : MonoBehaviour
{
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 25f;
    [SerializeField] Transform firePoint;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    
    [SerializeField] float fireRate = 1f;
    private float nextFireTime = 0f;

    public void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Debug.Log("Enemy fired the gun!");
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
        // Add a random offset to the direction of the raycast
        Vector3 direction = firePoint.forward + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
        if (Physics.Raycast(firePoint.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);
            // CreateHitImpact(hit);
            PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();

            if (hit.transform.CompareTag("PlayerBody") || hit.transform.CompareTag("Player"))
            {
                Debug.Log("Hit player!");
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Took damage off player!");
            }
        }
        else
        {
            Debug.Log("Missed player!");
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 1);
    }
}