using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManager objects if they exist
        }
    }
}
