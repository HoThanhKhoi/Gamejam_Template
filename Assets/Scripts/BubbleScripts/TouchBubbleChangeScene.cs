using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchBubbleChangeScene : MonoBehaviour
{
	[SerializeField] private int FirstLevelScene = 2;
	[SerializeField] private int LastLevelScene = 3;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			SceneManager.LoadScene(Random.Range(FirstLevelScene, LastLevelScene));
		}
	}
}
