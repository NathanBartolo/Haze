using UnityEngine;
using UnityEngine.SceneManagement; // Required to load scenes

public class PortalScript : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load
    public AudioClip portalSFX; // Drag your portal sound effect here
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure only the player triggers the portal
        {
            LoadNextScene();
            PlaySFX(portalSFX);
        }
    }

    void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect once
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad); // Load the specified scene
        }
        else
        {
            Debug.LogError("Scene name not specified for the portal!");
        }
    }
}
