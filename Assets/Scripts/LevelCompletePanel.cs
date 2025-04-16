using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SpaceVoyager
{
    public class LevelCompletePanel : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject panel;
        public TextMeshProUGUI levelCompleteText;
        public TextMeshProUGUI scoreText;
        public Button retryButton;
        public Button mainMenuButton;
        
        private void Start()
        {
            // Hide panel at start
            if (panel != null)
            {
                panel.SetActive(false);
            }
            
            // Set up button listeners
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(RetryLevel);
            }
            
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(ReturnToMainMenu);
            }
        }
        
        // This method will be called by the Signal Receiver
        public void ShowPanel()
        {
            if (panel != null)
            {
                panel.SetActive(true);
                
                // Pause the game
                Time.timeScale = 0f;
                
                // Update score text if available
                if (scoreText != null && GameManager.Instance != null)
                {
                    scoreText.text = "Score: " + GameManager.Instance.CurrentScore.ToString();
                }
            }
        }
        
        // Retry the current level
        public void RetryLevel()
        {
            // Reset game state
            GameManager.ResetGame();
            
            // Restore normal time scale
            Time.timeScale = 1f;
            
            // Hide the panel
            if (panel != null)
            {
                panel.SetActive(false);
            }
            
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        
        // Return to the main menu
        public void ReturnToMainMenu()
        {
            // Restore normal time scale before loading main menu
            Time.timeScale = 1f;
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadMainMenu();
            }
        }
    }
}
