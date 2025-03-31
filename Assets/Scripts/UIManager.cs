using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Add click listener programmatically as backup
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
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
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game..."); // Debug log to verify button click
        
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
