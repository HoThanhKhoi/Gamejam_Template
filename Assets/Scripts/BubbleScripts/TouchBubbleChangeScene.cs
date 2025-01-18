using UnityEngine;

public class TouchBubbleChangeScene : MonoBehaviour
{
	[SerializeField] private int FirstLevelScene = 2;
	[SerializeField] private int LastLevelScene = 4;

	// We'll reference the manager in the Inspector or via FindObjectOfType
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
		// pick random scene
		int nextScene = Random.Range(FirstLevelScene, LastLevelScene + 1);

		Debug.Log(nextScene);

		// Instead of SceneManager.LoadScene, we call:
		sceneManager.PlayVideoThenLoadScene(nextScene);
	}
}
