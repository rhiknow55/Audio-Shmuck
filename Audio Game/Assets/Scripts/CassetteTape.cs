using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteTape : MonoBehaviour {

	private AudioClip song;

	float enteredTime;
	float timeToStayInPodiumBeforeStartingSong = 3f;

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

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Podium")
		{
			enteredTime = Time.time;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.tag == "Podium")
		{
			float currentTime = Time.time;

			if(currentTime - enteredTime >= timeToStayInPodiumBeforeStartingSong)
			{
				if(!SongManager.songIsPlaying) PlayCassette();
			}
		}
	}
}
