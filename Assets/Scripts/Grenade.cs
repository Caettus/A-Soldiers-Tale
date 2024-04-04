using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionRadius = 5f;
    public int damage = 100;
    public ParticleSystem explosionEffect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Explode();
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        // Instantiate the explosion effect at the grenade's position
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}


