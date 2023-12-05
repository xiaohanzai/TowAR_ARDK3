using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource BuildingPlace;
	public AudioSource BuildingSpawn;
	public AudioSource PlatformPlace;
	public AudioSource BuildingDestroyed;
	public AudioSource BuildingSelect;
	public AudioSource Win;
	public AudioSource Loss;
	public AudioSource Network;
	public AudioSource Gather;
	public AudioSource EndGame;
	public AudioSource StartGame;
	public AudioSource Music;
	public AudioSource ButtonSelect;

	// Singleton instance.
	public static AudioManager Instance = null;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

	public void PlayWin()
	{
		Win.Play();
	}

	public void PlayLoss()
	{
		Loss.Play();
	}

	public void PlayNetwork()
	{
		Network.Play();
	}

	public void PlayGather()
	{
		Gather.Play();
	}

	public void PlayEndGame()
	{
		EndGame.Play();
	}

	public void PlayStartGame()
	{
		StartGame.Play();
	}

	public void PlayBuildingSelect()
	{
		BuildingSelect.Play();
	}

	public void PlayBuildingPlace()
	{
		BuildingPlace.Play();
	}
	public void PlayBuildingSpawn()
	{
		BuildingSpawn.Play();
	}
	public void PlayPlatformPlace()
	{
		PlatformPlace.Play();
	}
	public void PlayBuildingDestroyed()
	{
		BuildingDestroyed.Play();
	}

	public void PlayButtonSelect()
    {
		ButtonSelect.Play();
    }
	IEnumerator playSoundAfter2Seconds()
	{
		yield return new WaitForSeconds(2);
		Music.Play();
	}
	public void PlayMusic()
	{

		StartCoroutine(playSoundAfter2Seconds());
	}
	public void StopMusic()
	{
		//Music.Stop();
		StartCoroutine(FadeStop(Music));
	}

	IEnumerator FadeStop(AudioSource audioSource)
	{
		float startVol = audioSource.volume;

		while (audioSource.volume > 0)
		{
			audioSource.volume = Mathf.Max(0, audioSource.volume - 0.075f);
			yield return new WaitForSeconds(0.1f);
		}

		audioSource.Stop();
		audioSource.volume = startVol;
	}
}

