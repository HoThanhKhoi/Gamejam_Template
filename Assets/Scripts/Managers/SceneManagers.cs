using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;

public class SceneManagers : MonoBehaviour
{
	[Header("Video & Canvas Settings")]
	public GameObject rawImageObject;
	public RawImage rawImage;
	public VideoPlayer videoPlayer;
	public CanvasGroup canvasGroup;
	public Texture defaultTexture;
	public float fadeDuration = 1f;

	private static SceneManagers instance;
	private RenderTexture renderTexture;
	private AsyncOperation asyncOperation;

	public static SceneManagers Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindFirstObjectByType<SceneManagers>();
				if (instance == null)
				{
					Debug.LogError("SceneManagers instance not found!");
				}
			}
			return instance;
		}
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			SetupVideoPlayer();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void SetupVideoPlayer()
	{
		Time.timeScale = 1f;
		videoPlayer.playOnAwake = false;

		// Create a new RenderTexture for video playback
		renderTexture = new RenderTexture(1920, 1080, 0); // Adjust resolution as needed
		videoPlayer.targetTexture = renderTexture;
		rawImage.texture = renderTexture;

		videoPlayer.loopPointReached += OnVideoComplete;

		rawImageObject.SetActive(false);
		canvasGroup.alpha = 0f;
	}

	public void PlayVideoThenLoadScene(int sceneIndex)
	{
		StartCoroutine(TransitionSequence(sceneIndex));
	}

	private IEnumerator TransitionSequence(int sceneIndex)
	{
		rawImageObject.SetActive(true);
		Time.timeScale = 0f;

		// Ensure a valid RenderTexture is assigned
		if (renderTexture == null || !renderTexture.IsCreated())
		{
			renderTexture = new RenderTexture(1920, 1080, 0);
			videoPlayer.targetTexture = renderTexture;
			rawImage.texture = renderTexture;
		}

		// Start loading the next scene asynchronously
		asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
		asyncOperation.allowSceneActivation = false;

		// Play the video
		videoPlayer.Stop();
		videoPlayer.time = 0;
		videoPlayer.Play();

		// Fade in while the video plays
		yield return StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));

		// Wait for 5 seconds (or until the video finishes)
		yield return new WaitForSecondsRealtime(5f);

		// Ensure the scene is ready before activating
		while (asyncOperation.progress < 1f)
		{
			yield return null;
		}

		// Fade out and activate the scene
		yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));

		rawImageObject.SetActive(false);
		Time.timeScale = 1f;
		asyncOperation.allowSceneActivation = true;
	}

	private void OnVideoComplete(VideoPlayer vp)
	{
		if (asyncOperation != null && asyncOperation.progress >= 0.9f)
		{
			asyncOperation.allowSceneActivation = true;
		}
	}

	private IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.unscaledDeltaTime;
			float t = Mathf.Clamp01(elapsed / duration);
			canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
			yield return null;
		}
		canvasGroup.alpha = endAlpha;
	}

	private void OnDestroy()
	{
		if (videoPlayer != null)
		{
			videoPlayer.loopPointReached -= OnVideoComplete;
			CleanupVideoResources();
		}
	}

	private void CleanupVideoResources()
	{
		videoPlayer.Stop();
		videoPlayer.time = 0;

		if (renderTexture != null)
		{
			renderTexture.Release();
			renderTexture = new RenderTexture(1920, 1080, 0); // Recreate the RenderTexture
			videoPlayer.targetTexture = renderTexture;
			rawImage.texture = renderTexture;
		}
		else
		{
			rawImage.texture = defaultTexture;
		}

		rawImage.SetAllDirty();
	}
}
