using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SpaceVoyager
{
    public class HighScorePanel : MonoBehaviour
    {
        [Header("Panel References")]
        public GameObject highScorePanel;
        public TextMeshProUGUI highScoreText;
        public Button closeButton;
        public Button resetButton;
        
        [Header("UI Text")]
        public string highScorePrefix = "HIGH SCORE: ";
        
        private void Start()
        {
            // Hide panel at start
            if (highScorePanel != null)
                highScorePanel.SetActive(false);
            
            // Set up button listeners
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
                
            if (resetButton != null)
                resetButton.onClick.AddListener(ResetHighScore);
        }
        
        public void ShowHighScore()
        {
            // Load and display high score
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            
            if (highScoreText != null)
                highScoreText.text = highScorePrefix + highScore.ToString();
                
            // Show the panel
            if (highScorePanel != null)
                highScorePanel.SetActive(true);
        }
        
        public void ClosePanel()
        {
            if (highScorePanel != null)
                highScorePanel.SetActive(false);
        }
        
        private void ResetHighScore()
        {
            // Reset high score to 0
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
            
            // Update display
            if (highScoreText != null)
                highScoreText.text = highScorePrefix + "0";
                
            Debug.Log("High score reset to 0");
        }
    }
}
