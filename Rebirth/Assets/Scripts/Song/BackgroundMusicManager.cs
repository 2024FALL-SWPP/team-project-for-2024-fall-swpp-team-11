using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : SingletonManager<BackgroundMusicManager>
{
    public string firstScene = "MainMenu";
    public string secondScene = "Narration";

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake(); // Calls the base Singleton logic

        if (Instance == this) // Access through Instance, not _instance
        {
            // Add AudioSource component if not already attached
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Configure AudioSource
            audioSource.playOnAwake = true;
            audioSource.loop = true;

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    void Update()
    {
        // Destroy the GameObject if not in the allowed scenes
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != firstScene && currentScene != secondScene)
        {
            Destroy(gameObject);
        }
    }
}
