using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

public class SongChoiceMenuWebGL : EditorWindow {
	
    public static IEnumerator DesignateMusicFolder(Action onConvertClip)
    {
        string path = EditorUtility.OpenFolderPanel("Designate music folder", "", "");
        string[] files = Directory.GetFiles(path);

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fileInfos = dirInfo.GetFiles();

        foreach (string fileName in files)
        {
            if (fileName.EndsWith(".mp3") || fileName.EndsWith(".wav"))
            {
                Debug.Log("Path string is : " + fileName);
                AudioClip convertedClip = null;


                // If the file is an mp3, we first convert it to a wav.
                if (fileName.EndsWith(".mp3"))
                {
                    WWW www = new WWW(fileName);
                    while (!www.isDone)
                    {
                        yield return 0;
                    }

                    convertedClip = NAudioPlayer.FromMp3Data(fileName);
                }

                if (fileName.EndsWith(".wav"))
                {
                    WAV wav = new WAV(fileName);

                    convertedClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
                    convertedClip.SetData(wav.LeftChannel, 0);
                }

                if (convertedClip != null && !SongManager.instance.HasSong(convertedClip))
                {
                    AddSongToGlobalManager(convertedClip);
                }
            }
        }

        if (onConvertClip != null)
        {
            onConvertClip();
        }

        yield return null;

        //SongChoiceMenu.instance.SpawnCassettesFromSongs();
    }

    static void AddSongToGlobalManager(AudioClip clip)
    {
        SongManager.instance.AddSong(clip);
    }
}