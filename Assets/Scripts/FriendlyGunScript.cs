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

    // Add references to two audio clips for firing the gun
    [SerializeField] private AudioClip fireSound1;
    [SerializeField] private AudioClip fireSound2;
    // Add a reference to an AudioSource component
    [SerializeField] private AudioSource audioSource;

    public void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Debug.Log("Friendly unit fired the gun!");
            ProcessRaycast();
            PlayMuzzleFlash();
            PlayFireSound();
        }
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        // Add a random offset to the direction of the raycast for a more realistic aiming error
        Vector3 direction = firePoint.forward + new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), UnityEngine.Random.Range(-0.05f, 0.05f), UnityEngine.Random.Range(-0.05f, 0.05f));
        if (Physics.Raycast(firePoint.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemyHealth = hit.transform.GetComponent<Enemy>();

            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy!");
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log("Enemy took damage!");
                }
            }

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

    // Method to play one of the two fire sounds
    private void PlayFireSound()
    {
        if (audioSource != null)
        {
            // Randomly choose one of the two sounds
            AudioClip chosenClip = (Random.Range(0, 2) == 0) ? fireSound1 : fireSound2;
            audioSource.PlayOneShot(chosenClip);
        }
        else
        {
            Debug.LogError("AudioSource is not assigned on " + gameObject.name);
        }
    }
}
