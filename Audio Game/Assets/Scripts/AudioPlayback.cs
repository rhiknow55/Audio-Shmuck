using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Ryan Oh
/// 
/// 
/// </summary>

public class AudioPlayback : AbstractAudioCompiler
{
    public static AudioPlayback instance;

	bool isPlaying; // For the AtomicAttraction script to know to update or not.

	[Tooltip("The leeway given when inputting. Input must be within errorWindow before and after.")]
	public float errorWindow;

	void Awake()
	{
		// Check if instance already exists
		if (instance == null)
			// if not, set instance to this
			instance = this;
		// If instance already exists (not null) and it is not this
		else if (instance != this)
			// Then destroy this gameobject. This enforces our singleton pattern, meaning there can only ever be one instance of this manager
			Destroy(this.gameObject);
	}

	/// <summary>
	/// Begins the playback of "song" from "source"
	/// </summary>
	public void BeginPlayback(AudioClip clip, AudioSource source = null)
	{
		if (source != null)
		{
			audioSource = source;
		}

		audioSource.clip = clip;
		StartCoroutine(DelayPlayback());
	}

	private IEnumerator DelayPlayback()
	{
		yield return new WaitForSeconds(errorWindow);

		audioSource.Play();
		isPlaying = true;
	}

	public bool AudioIsPlaying()
	{
		return isPlaying;
	}
}
