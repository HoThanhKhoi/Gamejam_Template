using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUiManager : MonoBehaviour
{
	[SerializeField] private int FirstLevelScene = 2;
	[SerializeField] private int LastLevelScene = 4;

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	// Restarts the current game scene
	public void RestartGame()
	{
		int nextScene = Random.Range(FirstLevelScene, LastLevelScene + 1);

		SceneManager.LoadScene(nextScene);
	}
}
