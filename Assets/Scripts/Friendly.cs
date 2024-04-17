using UnityEngine;

public class Friendly : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    [SerializeField] private AudioClip[] deathSounds; // Array to hold the death sounds
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the object.");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log(health);
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Friendly died!");
        PlayDeathSound();
        Destroy(gameObject, audioSource.clip.length);
    }


    void PlayDeathSound()
    {
        if (deathSounds.Length > 0 && audioSource != null)
        {
            // Select a random sound clip from the array
            int index = Random.Range(0, deathSounds.Length);
            audioSource.clip = deathSounds[index];
            Debug.Log("Selected sound: " + audioSource.clip.name);
            audioSource.Play();
            Debug.Log("Playing sound: " + audioSource.clip.name);
        }
    }
}
