using System.Collections;
using UnityEngine;

public class StartSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        StartCoroutine(PlaySoundWithDelay(2.0f));
    }

    IEnumerator PlaySoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        
        audioSource.Play();
    }
}
