using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
	[Header("Scene Settings")]
	[Tooltip("The index or name of the scene to load.")]
	public int sceneToLoad = 1;

	private bool isTransitioning = false;

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !isTransitioning)
		{
			StartGame();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			QuitGame();
		}
	}

	private void StartGame()
	{
		isTransitioning = true;
		SceneManager.LoadScene(sceneToLoad);
	}

	private void QuitGame()
	{
		Debug.Log("Quitting game...");
		Application.Quit();
	}
}
