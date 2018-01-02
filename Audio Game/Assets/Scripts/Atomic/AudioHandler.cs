using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioHandler : MonoBehaviour 
{
	#region Variables
	public static AudioHandler instance;

	//public List<AudioClip> audioClips;
	public AudioClip selectedSong;

	[Tooltip("The leeway given when inputting. Input must be within errorWindow before and after.")]
	public float errorWindow;
	#endregion

	#region Monobehaviour Methods
	private void Awake()
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

	void Start()
	{
		InitAudioScripts();
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private/Protected Methods
	private void InitAudioScripts()
	{
		AudioCompiler.instance.StartAudioCompilation(selectedSong);

		StartCoroutine(DelayPlayback());
	}

	private IEnumerator DelayPlayback()
	{
		yield return new WaitForSeconds(errorWindow);
		
	}
	#endregion
}
