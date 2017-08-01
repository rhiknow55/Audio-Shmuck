using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// @author Ryan Oh
/// 
/// Holds all the songs in the project.
/// Manages AudioCompiler and AudioPlayback scripts.
/// </summary>
public class SongManager : MonoBehaviour {

	public static SongManager instance;

	public List<AudioClip> songs;
	AudioClip selectedSong;

	public static float playDelay = 3f;

	public static bool songIsPlaying;

	AudioPlayback leftPlayback, rightPlayback;
	AudioSource leftAudioSource, rightAudioSource;
	GameObject leftEarGO, rightEarGO;

	void Awake()
	{
		// The Start method is called once VRTK_SDKManager is initialized
		VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);

		// Check if instance already exists
		if (instance == null)
			// if not, set instance to this
			instance = this;
		// If instance already exists (not null) and it is not this
		else if (instance != this)
			// Then destroy this gameobject. This enforces our singleton pattern, meaning there can only ever be one instance of this manager
			Destroy(this.gameObject);

		// Sets this to not be destroyed when reloading or changing scenes.
		DontDestroyOnLoad(this.gameObject);
	}

	void OnDestroy()
	{
		VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
	}

	void OnStart()
	{
		InitAudioObjects();
	}

	/// <summary>
	/// Set the selected song to use for everything in this playthrough.
	/// </summary>
	/// <param name="song"></param>
	public void SetSelectedSong(AudioClip song)
	{
		print("Set selected song to " + song.name);
		selectedSong = song;
	}

	/// <summary>
	/// Starts the AudioCompiler playthrough of the set song. 
	/// Then after playDelay amount of time, starts actual playback.
	/// </summary>
	public void PlaySong()
	{
        print("Started Playing Song");
        AudioCompiler.instance.SetSongToCompile(selectedSong);
		Invoke("StartAudioPlayback", playDelay);

		songIsPlaying = true;
	}

	public void AddSong(AudioClip _clip)
	{
		songs.Add(_clip);
		print("Song added : " + _clip.name);
	}

	// Initialise the audiosources on the camera. Or create two GOs for spacial sound.
	void InitAudioObjects()
	{
		leftEarGO = new GameObject("Left Ear");
		rightEarGO = new GameObject("Right Ear");

		// Specify the transform the ears are at. Towards the head
		GameObject eyeCameraGO = GlobalManager.instance.GetEyeCameraGO();
		leftEarGO.transform.position = eyeCameraGO.transform.position;
		rightEarGO.transform.position = eyeCameraGO.transform.position;

		if (!leftEarGO.GetComponent<AudioSource>()) leftAudioSource = leftEarGO.AddComponent<AudioSource>();
		if (!rightEarGO.GetComponent<AudioSource>()) rightAudioSource = rightEarGO.AddComponent<AudioSource>();

		// Add the necessary AudioPlayback scripts to the ear GOs.
		if (!leftEarGO.GetComponent<AudioPlayback>()) leftPlayback = leftEarGO.AddComponent<AudioPlayback>();
		if (!rightEarGO.GetComponent<AudioPlayback>()) rightPlayback = rightEarGO.AddComponent<AudioPlayback>();
	}

	// Starts the corresponding AudioPlaybacks
	void StartAudioPlayback()
	{
		leftPlayback.BeginPlayback(selectedSong, leftAudioSource);
		rightPlayback.BeginPlayback(selectedSong, rightAudioSource);
	}
}
