using UnityEngine;
using UnityEngine.Audio;
using Utils;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource sfxSource;

	public AudioClip backgroundMusic;
	public AudioClip movementSound;
	public AudioClip shootSound;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	private void Start()
	{
		musicSource.clip = backgroundMusic;
		musicSource.Play();
	}
	public void PlayMovementSound()
	{
		sfxSource.clip = movementSound;
		sfxSource.Play();
	}
	public void PlayShootSound()
	{
		sfxSource.clip = shootSound;
		sfxSource.Play();
	}
}
