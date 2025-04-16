using UnityEngine;
using TMPro;
using SpaceVoyager;  // Add reference to our namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button mainMenuButton;  // Add reference for main menu button
    
    // Score UI elements
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    
    // References to score panels
    public GameObject currentScorePanel;
    public GameObject highScorePanel;
    
    // Add delay for game over panel
    public float gameOverDelay = 1.5f; // Adjust this value based on your explosion animation length
    private bool isShowingGameOver = false;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Add click listeners programmatically as backup
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
            
        // Initialize score text
        UpdateScoreDisplay();
        
        // Make sure score panels are visible at start
        ShowScorePanels(true);
    }

    void Update()
    {
        // Show game over panel when the game is over
        if (SpaceshipController.IsGameOver && gameOverPanel != null && !gameOverPanel.activeSelf && !isShowingGameOver)
        {
            isShowingGameOver = true;
            StartCoroutine(ShowGameOverDelayed());
        }
        
        // Update score display every frame
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        // Only update if we have references to the text components
        if (currentScoreText != null && GameManager.Instance != null)
        {
            currentScoreText.text = GameManager.Instance.CurrentScore.ToString();
        }
        
        if (highScoreText != null && GameManager.Instance != null)
        {
            highScoreText.text = GameManager.Instance.HighScore.ToString();
        }
    }

    private IEnumerator ShowGameOverDelayed()
    {
        // Wait for the explosion animation to complete
        yield return new WaitForSecondsRealtime(gameOverDelay);
        
        // Now show the game over panel
        ShowGameOver();
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            // Hide score panels
            ShowScorePanels(false);
            
            // Show game over panel
            gameOverPanel.SetActive(true);
            if (gameOverText != null)
                gameOverText.text = "GAME OVER";
            Time.timeScale = 0f; // Pause the game
        }
    }
    
    // Helper method to show/hide score panels
    private void ShowScorePanels(bool show)
    {
        if (currentScorePanel != null)
            currentScorePanel.SetActive(show);
            
        if (highScorePanel != null)
            highScorePanel.SetActive(show);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game..."); // Debug log to verify button click
        
        // Explicitly reset the game state
        GameManager.ResetGame();
        isShowingGameOver = false;
        
        // Reset time scale
        Time.timeScale = 1f;
        
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu..."); // Debug log to verify button click
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene("MainMenu");
    }
}
