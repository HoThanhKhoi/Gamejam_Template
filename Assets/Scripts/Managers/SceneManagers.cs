using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;
using Utils;

public class SceneManagers : Singleton<SceneManagers>
{
	[Header("Video & Canvas Settings")]
	public VideoPlayer videoPlayer;      // Assign the VideoPlayer here
	public CanvasGroup canvasGroup;      // Assign a CanvasGroup for fade effects
	public float fadeDuration = 1f;      // Time (seconds) to fade in/out the video overlay

	private bool videoFinished = false;

	void Awake()
	{
		// Make this manager persist between scenes
		DontDestroyOnLoad(gameObject);

		// Attach an event to detect when the video finishes playing
		videoPlayer.loopPointReached += OnVideoFinished;
	}

	/// <summary>
	/// Start the video transition, freeze the scene, and load the next scene.
	/// </summary>
	/// <param name="sceneIndex">Index of the scene to load.</param>
	public void PlayVideoThenLoadScene(int sceneIndex)
	{
		StartCoroutine(TransitionSequence(sceneIndex));
	}

	/// <summary>
	/// Transition sequence to fade in, play the video, and load the new scene.
	/// Freezes everything in the current scene while the video plays.
	/// </summary>
	private IEnumerator TransitionSequence(int sceneIndex)
	{
		// Freeze the scene
		Time.timeScale = 0f;

		// Reset video and ensure it starts at the beginning
		videoPlayer.Stop();
		videoPlayer.time = 0;
		videoFinished = false; // Reset the flag
		videoPlayer.Play();

		// Fade in the video canvas (from alpha 0 to 1)
		yield return StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));

		// Wait for the video to finish playing
		while (!videoFinished)
		{
			yield return null; // Wait until OnVideoFinished is triggered
		}

		// Load the next scene while the video canvas is still fully visible (alpha=1)
		SceneManager.LoadScene(sceneIndex);

		// Fade out the video canvas (from alpha 1 to 0) after the scene loads
		yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));

		// Stop the video and reset it for future transitions
		videoPlayer.Stop();
		videoPlayer.time = 0;

		// Unfreeze the scene
		Time.timeScale = 1f;
	}

	/// <summary>
	/// Called when the video ends (triggered by loopPointReached event).
	/// </summary>
	private void OnVideoFinished(VideoPlayer vp)
	{
		videoFinished = true; // Mark the video as finished
	}

	/// <summary>
	/// Fades the canvas group over time.
	/// </summary>
	private IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
	{
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.unscaledDeltaTime; // Use unscaled time to avoid being paused
			float t = Mathf.Clamp01(elapsed / duration);
			canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
			yield return null;
		}

		canvasGroup.alpha = endAlpha; // Ensure final alpha is exact
	}
}
