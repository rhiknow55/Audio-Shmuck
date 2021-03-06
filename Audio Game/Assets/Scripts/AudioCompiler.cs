﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Ryan Oh
/// 
/// </summary>

public class AudioCompiler : AbstractAudioCompiler
{
	public static AudioCompiler instance;

	private bool isPlaying;

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
	/// Specify the song to compile for this playthrough.
	/// </summary>
	public void StartAudioCompilation(AudioClip _song)
	{
		audioSource.clip = _song;
		audioSource.Play();
		isPlaying = true;
	}

	public bool AudioIsPlaying()
	{
		return isPlaying;
	}
}
