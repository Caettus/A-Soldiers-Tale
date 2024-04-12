using UnityEngine;
using System.Collections;

public class GasAttackManager : MonoBehaviour
{
    public GameObject gasCloudPrefab;
    public BoxCollider attackArea; 
    public float minTimeBetweenAttacks = 10f;
    public float maxTimeBetweenAttacks = 30f;

    private void Start()
    {
        StartCoroutine(SpawnGasAttacks());
    }

    private IEnumerator SpawnGasAttacks()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks));
            Vector3 attackPosition = GenerateRandomPointInArea(attackArea);
            Instantiate(gasCloudPrefab, attackPosition, Quaternion.identity);
        }
    }

    Vector3 GenerateRandomPointInArea(BoxCollider area)
    {
        Vector3 boundsMin = area.bounds.min;
        Vector3 boundsMax = area.bounds.max;
        float x = Random.Range(boundsMin.x, boundsMax.x);
        float z = Random.Range(boundsMin.z, boundsMax.z);
        float y = area.transform.position.y;
        return new Vector3(x, y, z);
    }
}

