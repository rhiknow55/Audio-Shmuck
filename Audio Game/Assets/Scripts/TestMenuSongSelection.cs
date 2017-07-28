using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SongNumber
{
	ONE = 0,
	TWO = 1,
	THREE = 2,
	FOUR = 3,
	FIVE = 4
}

public class TestMenuSongSelection : MonoBehaviour {

	public SongNumber chosenSongNumber;

	void Start()
    {
        AudioClip song = SongManager.instance.songs[(int)chosenSongNumber];
        GlobalManager.instance.SetSelectedSong(song);
        
    }
}
