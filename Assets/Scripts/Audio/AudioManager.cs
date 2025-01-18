using UnityEngine;
using UnityEngine.Audio;
using Utils;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource movementSource;

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
        if (!movementSource.isPlaying || movementSource.clip != movementSound)
        {
            movementSource.clip = movementSound;
            movementSource.loop = true;
            movementSource.Play();
        }
    }

    public void StopMovementSound()
    {
        if (movementSource.clip == movementSound)
        {
            movementSource.Stop();
            movementSource.loop = false;
        }
    }

    public bool IsPlayingMovementSound()
    {
        return movementSource.isPlaying && movementSource.clip == movementSound;
    }

    public void PlayShootSound()
	{
        sfxSource.PlayOneShot(shootSound);
    }
}
