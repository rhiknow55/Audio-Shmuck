using System.Collections;
using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SongChoiceMenuAndroid : MonoBehaviour
{
	public static SongChoiceMenuAndroid instance;

	public void Awake()
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

	public IEnumerator DesignateMusicFolder(Action onConvertClip)
	{
		string path = GetAndroidInternalFilesDir();
		//DebugLogger.instance.Log("Path: " + path);
		if (string.IsNullOrEmpty(path))
			DebugLogger.instance.Log("Empty path");

		string[] files = Directory.GetFiles(path);
		//DebugLogger.instance.Log("# of files : " + files.Length);

		int amount = files.Length;
		for (int i = 0; i < amount; ++i)
		{
			string fullFileName = "file://" + files[i];
			DebugLogger.instance.Log(fullFileName + " : " + i + " : " + amount);

			if (fullFileName.EndsWith(".mp3") || fullFileName.EndsWith(".wav"))
			{
				AudioClip convertedClip = null;

				// If the file is an mp3, we first convert it to a wav.
				if (fullFileName.EndsWith(".mp3"))
				{
					WWW www = new WWW(fullFileName);
					while (!www.isDone)
					{
						
					}

					//convertedClip = NAudioPlayer.FromMp3Data(fullFileName);

					convertedClip = www.GetAudioClip(false);
				}
				else if (fullFileName.EndsWith(".wav"))
				{
					WAV wav = new WAV(fullFileName);

					convertedClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
					convertedClip.SetData(wav.LeftChannel, 0);
				}

				DebugLogger.instance.Log("Converted clip: " + convertedClip.length);
				if (convertedClip == null)
					DebugLogger.instance.Log("was null");
				
				AddClipsToSongManager(convertedClip);
			}
			DebugLogger.instance.Log("wat");
		}

		if (onConvertClip != null)
		{
			onConvertClip();
		}

		yield return null;
	}

	public string GetAndroidInternalFilesDir()
	{
		string[] potentialDirectories = new string[]
		{
			"/mnt/sdcard", // Exists
			"file:///sdcard/Music",
			"file://mnt/sdcard/Music",
			"/mnt/sdcard/Music", // Exists + has music
			"///sdcard/Music", // Exists + has music
			"/sdcard", // Exists
			"/sdcard/Music", // Exists + has music
			"/storage/sdcard0",
			"/storage/sdcard1",
			"/storage/sdcard0/Music",
			"/storage/sdcard1/Music"
		};

		List<string> existingDirectories = new List<string>();

		if (Application.platform == RuntimePlatform.Android)
		{
			for (int i = 0; i < potentialDirectories.Length; i++)
			{
				if (Directory.Exists(potentialDirectories[i]))
				{
					string potentialDirectory = potentialDirectories[i];
					//DebugLogger.instance.Log("Existing directory: " + potentialDirectory);

					string[] files = Directory.GetFiles(potentialDirectory);

					foreach (string fileName in files)
					{
						if (fileName.EndsWith(".mp3") || fileName.EndsWith(".wav"))
						{
							existingDirectories.Add(potentialDirectory);
							//DebugLogger.instance.Log(potentialDirectory + " contains audio files");
							break;
						}
					}
				}
			}
		}
		return existingDirectories.Count == 0 ? "" : existingDirectories[0];
	}

	private void AddClipsToSongManager(AudioClip clip)
	{
		SongManager.instance.AddSong(clip);
	}
}
