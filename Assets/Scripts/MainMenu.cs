using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
}
