using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUiManager : MonoBehaviour
{
	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	// Restarts the current game scene
	public void RestartGame()
	{
		SceneManager.LoadScene(1);
	}
}
