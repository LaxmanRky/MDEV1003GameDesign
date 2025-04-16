using UnityEngine;
using TMPro;

namespace SpaceVoyager
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("Score Text References")]
        public TextMeshProUGUI currentScoreLabel;
        public TextMeshProUGUI currentScoreValue;
        public TextMeshProUGUI highScoreLabel;
        public TextMeshProUGUI highScoreValue;

        [Header("UI Positioning")]
        public RectTransform currentScoreContainer;
        public RectTransform highScoreContainer;
        
        [Header("UI Styling")]
        public Color scoreTextColor = Color.white;
        public Color highScoreTextColor = Color.yellow;
        
        private UIManager uiManager;

        private void Awake()
        {
            // Find UI Manager
            uiManager = FindObjectOfType<UIManager>();
            
            // Set up UI references
            if (uiManager != null)
            {
                if (currentScoreValue != null)
                {
                    uiManager.currentScoreText = currentScoreValue;
                }
                
                if (highScoreValue != null)
                {
                    uiManager.highScoreText = highScoreValue;
                }
            }
            
            // Set up text labels
            if (currentScoreLabel != null)
            {
                currentScoreLabel.text = "SCORE";
                currentScoreLabel.color = scoreTextColor;
            }
            
            if (currentScoreValue != null)
            {
                currentScoreValue.color = scoreTextColor;
            }
            
            if (highScoreLabel != null)
            {
                highScoreLabel.text = "HIGH SCORE";
                highScoreLabel.color = highScoreTextColor;
            }
            
            if (highScoreValue != null)
            {
                highScoreValue.color = highScoreTextColor;
            }
        }
    }
}
