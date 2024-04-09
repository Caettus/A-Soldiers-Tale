using System.Collections;
using UnityEngine;

public class ArtilleryStrike : MonoBehaviour {
    public GameObject indicatorPrefab; 
    public GameObject explosionPrefab; 
    public BoxCollider strikeArea; 
    public float minTimeBetweenStrikes = 5f;
    public float maxTimeBetweenStrikes = 10f;
    public float explosionRadius = 5f;
    public CameraShake cameraShake; 
    [SerializeField] private float shakeRadius = 20f;

    void Start() {
        StartCoroutine(ArtilleryStrikeRoutine());
    }

    IEnumerator ArtilleryStrikeRoutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenStrikes, maxTimeBetweenStrikes));
            Vector3 strikePosition = GenerateRandomPointInArea(strikeArea);
            StartCoroutine(ShowIndicatorAndExplode(strikePosition));
        }
    }

    Vector3 GenerateRandomPointInArea(BoxCollider area) {
        Vector3 boundsMin = area.bounds.min;
        Vector3 boundsMax = area.bounds.max;

        
        float x = Random.Range(boundsMin.x, boundsMax.x);
        float z = Random.Range(boundsMin.z, boundsMax.z);
        float y = area.transform.position.y;

        return new Vector3(x, y, z);
    }

    IEnumerator ShowIndicatorAndExplode(Vector3 position) {
        GameObject indicator = Instantiate(indicatorPrefab, position, Quaternion.identity);
        yield return new WaitForSeconds(2); 
        Destroy(indicator); 
        TriggerExplosion(position); 
    }

    void TriggerExplosion(Vector3 position) {
        GameObject explosionEffect = Instantiate(explosionPrefab, position, Quaternion.identity);
        Destroy(explosionEffect, 5); 
        
        // Camera shake
        float distanceToCamera = Vector3.Distance(Camera.main.transform.position, position);

        if (distanceToCamera <= shakeRadius) {
           
            float shakeDuration = 0.5f; 
            float shakeMagnitude = 0.2f; 
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
        }
        
        // Damage players in the area
        Collider[] hitColliders = Physics.OverlapSphere(position, explosionRadius);
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.CompareTag("Player")) {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null) {
                    playerHealth.TakeDamage(75); 
                    Debug.Log("Player hit by artillery strike!");
                    Debug.Log(playerHealth.health);
                }
            }
        }
    }
}
