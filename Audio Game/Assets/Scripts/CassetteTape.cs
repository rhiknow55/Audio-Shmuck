using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteTape : MonoBehaviour {

	private AudioClip song;

	/// <summary>
	/// Set the song that this cassette tape will play.
	/// </summary>
	/// <param name="_song"></param>
	public void SetReferenceSong(AudioClip _song)
	{
		song = _song;
	}


	public void PlayCassette()
	{
		SongManager.instance.SetSelectedSong(song);
		SongManager.instance.PlaySong();
	}
}
