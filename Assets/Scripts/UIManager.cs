using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
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
        // Add restart functionality here
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
