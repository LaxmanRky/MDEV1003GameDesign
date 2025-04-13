using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SVGameScene");
        GameManager.ResetGame(); // Reset game state when starting new game
    }
}
