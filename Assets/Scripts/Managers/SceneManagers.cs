using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

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
				instance = FindAnyObjectByType<SceneManagers>();
				if (instance == null)
				{
					Debug.LogError("SceneManagers instance not found!");
				}
			}
			return instance;
		}
	}

	private void Awake()
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

		// Randomize post-processing before transitioning
		RandomizePostProcessing();

		// Ensure a valid RenderTexture is assigned
		if (renderTexture == null || !renderTexture.IsCreated())
		{
			renderTexture = new RenderTexture(1920, 1080, 0);
			videoPlayer.targetTexture = renderTexture;
			rawImage.texture = renderTexture;
		}

		// Start loading the next scene asynchronously using the provided index
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
		while (asyncOperation.progress < 0.9f)
		{
			yield return null;
		}

		// Fade out and activate the scene
		yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));

		rawImageObject.SetActive(false);
		Time.timeScale = 1f;
		asyncOperation.allowSceneActivation = true;
	}

	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
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

	private void RandomizePostProcessing()
	{
		// Find the global volume in the scene
		Volume globalVolume = FindAnyObjectByType<Volume>();

		if (globalVolume == null)
		{
			Debug.LogWarning("No global volume found in the scene!");
			return;
		}

		VolumeProfile profile = globalVolume.profile;

		if (profile == null)
		{
			Debug.LogWarning("No volume profile found on the global volume!");
			return;
		}

		// Iterate through overrides and randomize them
		foreach (var component in profile.components)
		{
			if (component is Bloom bloom)
			{
				bloom.intensity.Override(Random.Range(0.1f, 5f)); // Random intensity between 0 and 5
			}
			else if (component is LensDistortion lensDistortion)
			{
				lensDistortion.intensity.Override(Random.Range(-0.5f, 0.5f));    // Random contrast
			}
			else if (component is Vignette vignette)
			{
				vignette.intensity.Override(Random.Range(0f, 0.5f)); // Random vignette intensity
			}
			else if (component is ChromaticAberration chromaticAberration)
			{
				chromaticAberration.intensity.Override(Random.Range(0f, 0.7f)); // Random chromatic aberration intensity
			}
			else if (component is FilmGrain filmGrain)
			{
				filmGrain.intensity.Override(Random.Range(0f, 1f)); // Random film grain intensity
			}
		}

		Debug.Log("Post-processing effects randomized!");
	}
}
