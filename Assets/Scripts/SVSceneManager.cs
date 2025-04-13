using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SVSceneManager : MonoBehaviour
{
    public GameObject gameOverCanvas; // Assign the game over UI canvas in inspector
    public Button mainMenuButton;     // Assign the main menu button in inspector

    void Start()
    {
        // Make sure canvas is hidden at start
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        // Add click listener to main menu button
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before changing scene
        SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        // Clean up listener when object is destroyed
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
        }
    }
}
