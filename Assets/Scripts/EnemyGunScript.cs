using System;
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

    [SerializeField] AudioClip gunSound1; // First gun sound
    [SerializeField] AudioClip gunSound2; // Second gun sound
    [SerializeField] AudioSource audioSource; // AudioSource component

    public void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Debug.Log("Enemy fired the gun!");
            ProcessRaycast();
            PlayMuzzleFlash();
            PlayGunSound(); // Play a gun sound
        }
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    void ProcessRaycast()
    {
        RaycastHit hit;
        Vector3 direction = firePoint.forward + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
        if (Physics.Raycast(firePoint.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
            Friendly friendlyHealth = hit.transform.GetComponent<Friendly>();

            if (hit.transform.CompareTag("PlayerBody") || hit.transform.CompareTag("Player") || hit.transform.CompareTag("Friendly"))
            {
                Debug.Log("Hit someone!");
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Took damage off player HAHAHA!");
            }
            if (friendlyHealth != null)
            {
                friendlyHealth.TakeDamage(damage);
                Debug.Log("Took damage off someone!");
            }
        }
        else
        {
            Debug.Log("Missed someone!");
        }
    }

    private void PlayGunSound()
    {
        AudioClip clipToPlay = UnityEngine.Random.value > 0.5f ? gunSound1 : gunSound2;
        audioSource.PlayOneShot(clipToPlay);
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 1);
    }
}
