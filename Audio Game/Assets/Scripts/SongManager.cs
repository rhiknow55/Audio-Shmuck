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

	public AudioClip[] songs;

	public static float playDelay = 3f;

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
	/// Starts the AudioCompiler playthrough of the set song. 
	/// Then after playDelay amount of time, starts actual playback.
	/// </summary>
	public void StartSong()
	{
        print("Started Playing Song");
        AudioCompiler.instance.SetSongToCompile(GlobalManager.instance.GetSelectedSong());
		Invoke("StartAudioPlayback", playDelay);
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
		leftPlayback.BeginPlayback(GlobalManager.instance.GetSelectedSong(), leftAudioSource);
		rightPlayback.BeginPlayback(GlobalManager.instance.GetSelectedSong(), rightAudioSource);
	}
}
