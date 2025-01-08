using UnityEngine;

public class RealmManager : MonoBehaviour
{
    public bool isLightRealm = true; // Starts in the Light realm

    public GameObject[] lightPlatforms; // Array of light realm platforms
    public GameObject[] shadowTraps;    // Array of shadow realm traps

    public Color lightRealmColor = Color.white; // Background color for Light Realm
    public Color shadowRealmColor = Color.black; // Background color for Shadow Realm

    public Color lightFogColor = new Color(1f, 1f, 1f, 0.5f); // Light realm fog color
    public Color shadowFogColor = new Color(0.1f, 0.1f, 0.1f, 0.8f); // Shadow realm fog color
    public float lightFogDensity = 0.01f; // Light realm fog density
    public float shadowFogDensity = 0.04f; // Shadow realm fog density
    public AudioClip RealmSFX; // Drag your realm switch sound effect here
    private AudioSource audioSource;

    private Camera mainCamera; // Reference to the Main Camera

    void Start()
    {
        // Find the Main Camera in the scene
        mainCamera = Camera.main;

        // Set initial background color and fog
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = lightRealmColor;
        }

        RenderSettings.fog = true; // Enable fog globally
        RenderSettings.fogColor = lightFogColor;
        RenderSettings.fogDensity = lightFogDensity;

        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the RealmManager GameObject!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Switch realms on L key
        {
            ToggleRealm();
            PlaySFX(RealmSFX);
        }
    }

    void ToggleRealm()
    {
        isLightRealm = !isLightRealm; // Toggle the realm state

        // Change the background color of the camera
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = isLightRealm ? lightRealmColor : shadowRealmColor;
        }

        // Change fog settings
        if (isLightRealm)
        {
            RenderSettings.fogColor = lightFogColor;
            RenderSettings.fogDensity = lightFogDensity;
        }
        else
        {
            RenderSettings.fogColor = shadowFogColor;
            RenderSettings.fogDensity = shadowFogDensity;
        }

        // Update platforms and traps based on the new realm
        if (isLightRealm)
        {
            UpdateLightRealmObjects(true);  // Enable platforms in Light Realm (solid and visible)
            UpdateShadowRealmObjects(false); // Disable traps in Light Realm
        }
        else
        {
            UpdateLightRealmObjects(false); // Disable platforms in Shadow Realm (invisible and non-solid)
            UpdateShadowRealmObjects(true); // Enable traps in Shadow Realm (visible but deadly)
        }

        Debug.Log("Switched to " + (isLightRealm ? "Light Realm" : "Shadow Realm"));
    }

    // Update light platforms based on realm status
    void UpdateLightRealmObjects(bool active)
    {
        // Find all platforms with the tag "LightPlatform"
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("LightPlatform");

        foreach (GameObject platform in platforms)
        {
            // Get the BoxCollider2D component
            BoxCollider2D collider = platform.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = active; // Enable or disable the collider based on realm
            }

            // Update visibility based on realm
            SpriteRenderer renderer = platform.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = active; // Set visibility
            }
        }
    }

    // Update shadow traps based on realm status
    void UpdateShadowRealmObjects(bool active)
    {
        foreach (GameObject trap in shadowTraps)
        {
            // Update visibility for shadow traps
            SpriteRenderer renderer = trap.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = active; // Set visibility based on realm
            }

            // Update the collider status for shadow traps
            BoxCollider2D collider = trap.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = active; // Make traps deadly and active in the Shadow Realm
            }
        }
    }

    // Play sound effect
    void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect once
        }
    }
}
