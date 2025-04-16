using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceVoyager;

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

    // Score tracking
    private int currentScore = 0;
    private int highScore = 0;
    private float scoreTimer = 0f;
    private float scoreIncreaseRate = 1f; // Increase score every second

    // Properties to access scores
    public int CurrentScore => currentScore;
    public int HighScore => highScore;

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
        
        // Load high score from PlayerPrefs
        LoadHighScore();
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

    private void Update()
    {
        // Only update score if game is active
        if (!IsGameOver && SceneManager.GetActiveScene().name != "MainMenu")
        {
            // Increase score over time
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= scoreIncreaseRate)
            {
                scoreTimer = 0f;
                IncreaseScore(1);
            }
        }
    }

    // Method to increase the score
    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        
        // Update high score if needed
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
        }
    }

    // Reset the current score
    public void ResetScore()
    {
        currentScore = 0;
    }

    // Save high score to PlayerPrefs
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    // Load high score from PlayerPrefs
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void GameOver()
    {
        IsGameOver = true;
        
        // Check if we have a new high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
        }
        
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
        
        // Reset score when starting a new game
        if (Instance != null)
        {
            // Explicitly reset current score to 0
            Instance.currentScore = 0;
            Instance.scoreTimer = 0f;
            Debug.Log("Score reset to 0");
        }
    }
}
