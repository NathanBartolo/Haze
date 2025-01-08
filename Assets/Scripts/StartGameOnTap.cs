using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro; // If using TextMeshPro

public class StartGameOnKeyPress : MonoBehaviour
{
    public string sceneToLoad = "MainScene"; // Name of the scene to load
    public TextMeshProUGUI startText; // Reference to the "Press Any Key to Start" text
    private CanvasGroup canvasGroup;
    public AudioClip startGameSFX; // Drag your sound effect here
    private AudioSource audioSource;

    void Start()
    {
        canvasGroup = startText.GetComponent<CanvasGroup>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Detect any key press
        if (Input.anyKeyDown)
        {
            StartCoroutine(FadeAndStart());
            PlaySFX(startGameSFX);
        }
    }

    void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect once
        }
    }

    IEnumerator FadeAndStart()
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Load the next scene after fading out
        SceneManager.LoadScene(sceneToLoad);
    }
}
