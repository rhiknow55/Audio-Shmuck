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
	protected override void Start()
	{
		base.Start();

		
	}

	/// <summary>
	/// Begins the playback of "song" from "source"
	/// </summary>
	/// <param name="_song"></param>
	/// <param name="_source"></param>
	public void BeginPlayback(AudioClip _song, AudioSource _source)
	{
		_source.clip = _song;
		_source.Play();
	}
}
