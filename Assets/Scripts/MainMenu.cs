using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Reference to the settings panel

    private void Start()
    {
        // Make sure settings panel is hidden at start
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
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
        }
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}
