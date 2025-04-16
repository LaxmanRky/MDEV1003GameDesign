using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceVoyager
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject settingsPanel; // Reference to the settings panel
        public GameObject highScorePanel; // Reference to the high score panel
        private HighScorePanel highScorePanelScript; // Reference to the high score panel script

        private void Start()
        {
            // Make sure settings panel is hidden at start
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
            
            // Make sure high score panel is hidden at start
            if (highScorePanel != null)
            {
                highScorePanel.SetActive(false);
                // Get the HighScorePanel script component
                highScorePanelScript = highScorePanel.GetComponent<HighScorePanel>();
            }
        }

        public void StartGame()
        {
            Debug.Log("StartGame method called");
            try
            {
                SceneManager.LoadScene("SVGameScene");
                GameManager.ResetGame(); // Reset game state when starting new game
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load scene: {e.Message}");
            }
        }

        public void ToggleSettingsPanel()
        {
            if (settingsPanel != null)
            {
                // If panel is active, hide it. If it's hidden, show it
                settingsPanel.SetActive(!settingsPanel.activeSelf);
                
                // Hide high score panel if it's active
                if (highScorePanel != null && highScorePanel.activeSelf)
                {
                    highScorePanel.SetActive(false);
                }
            }
        }

        public void CloseSettingsPanel()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }
        
        public void ShowHighScorePanel()
        {
            // Hide settings panel if it's active
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(false);
            }
            
            // Show high score panel and update the score
            if (highScorePanelScript != null)
            {
                highScorePanelScript.ShowHighScore();
            }
            else if (highScorePanel != null)
            {
                highScorePanel.SetActive(true);
            }
        }

        public void ExitGame()
        {
            Debug.Log("Exiting game...");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
