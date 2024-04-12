using UnityEngine;
using System.Collections;

public class ExpandGasCloud : MonoBehaviour
{
    public Vector3 expansionRate = new Vector3(0.1f, 0.1f, 0.1f); // Rate of expansion per second
    public float expansionDuration = 30f; // How long the cloud will expand
    private float currentDuration = 0f;

    private void Update()
    {
        if (currentDuration < expansionDuration)
        {
            transform.localScale += expansionRate * Time.deltaTime;
            currentDuration += Time.deltaTime;
        }
    }
}
