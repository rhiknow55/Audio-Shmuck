using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuSongSelection : MonoBehaviour {

    void Start()
    {
        AudioClip song = SongManager.instance.songs[1];
        GlobalManager.instance.SetSelectedSong(song);
        SongManager.instance.StartSong();
    }
}
