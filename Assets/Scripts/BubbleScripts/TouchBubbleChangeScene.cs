using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchBubbleChangeScene : MonoBehaviour
{
	[SerializeField] private int FirstLevelScene = 2;
	[SerializeField] private int LastLevelScene = 4;

	// We'll reference the manager in the Inspector or via FindObjectByType
	[SerializeField] private SceneManagers sceneManager;

	private bool isChangingScene = false;

	void Awake()
	{
		if (!sceneManager)
			sceneManager = FindFirstObjectByType<SceneManagers>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !isChangingScene)
		{
			isChangingScene = true;
			LoadSceneWithVideo();
		}
	}

	private void LoadSceneWithVideo()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextScene;

		// Keep picking a random scene until it's not the current scene
		do
		{
			nextScene = Random.Range(FirstLevelScene, LastLevelScene +2);
		}
		while (nextScene == currentSceneIndex);

		Debug.Log("Next Scene: " + nextScene);

		// Instead of SceneManager.LoadScene, we call:
		sceneManager.PlayVideoThenLoadScene(nextScene);
	}
}
