using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button mainMenuButton;  // Add reference for main menu button

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Add click listeners programmatically as backup
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    void Update()
    {
        // Show game over panel when the game is over
        if (SpaceshipController.IsGameOver && gameOverPanel != null && !gameOverPanel.activeSelf)
        {
            ShowGameOver();
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (gameOverText != null)
                gameOverText.text = "GAME OVER";
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game..."); // Debug log to verify button click
        Time.timeScale = 1f; // Reset time scale
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
