using System.Collections;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public GameObject grenadePrefab;
    public float throwForce = 10f;
    public float cooldownTime = 2f;
    private float nextThrowTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && Time.time > nextThrowTime)
        {
            ThrowGrenade();
            nextThrowTime = Time.time + cooldownTime;
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position + transform.forward, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 throwDirection = transform.forward + transform.up;
            rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
        }
    }
}
