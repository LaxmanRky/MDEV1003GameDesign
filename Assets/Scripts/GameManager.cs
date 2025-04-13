using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Ensure we have an audio listener
        SetupAudioListener();
    }

    private void SetupAudioListener()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        
        if (listeners.Length == 0)
        {
            // Try to find the main camera first
            Camera mainCamera = Camera.main;
            if (mainCamera != null && mainCamera.GetComponent<AudioListener>() == null)
            {
                mainCamera.gameObject.AddComponent<AudioListener>();
                Debug.Log("Added AudioListener to main camera");
            }
            else
            {
                // If no main camera or it already has a listener, add to this game manager
                gameObject.AddComponent<AudioListener>();
                Debug.Log("Added AudioListener to GameManager");
            }
        }
        else if (listeners.Length > 1)
        {
            // Remove extra listeners if there are more than one
            Debug.LogWarning("Found multiple AudioListeners in the scene. Removing extras.");
            for (int i = 1; i < listeners.Length; i++)
            {
                Destroy(listeners[i]);
            }
        }
    }

    public static bool IsGameOver { get; set; }

    public void GameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void ResetGame()
    {
        IsGameOver = false;
        Time.timeScale = 1f;
    }
}
