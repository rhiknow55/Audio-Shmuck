using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Begins the acquisition of songs.
/// Creates the cassettes from the pool of songs in SongManager.
/// Plays the song once it is chosen.
/// </summary>
public class SongChoiceMenu : MonoBehaviour {

	public static SongChoiceMenu instance;

	public GameObject cassettePrefab;
	public Transform cassetteSpawn;

	List<GameObject> cassettes;

	void Awake()
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
		if(!VRorFPS.instance.usingVR) SpawnCassettesFromSongs();

	}

	// Opens the windows menu for the player to choose a folder with songs. Attach this method to a button of sorts.
	public void OpenFolderPanel()
	{
		StartCoroutine(SongChoiceMenuEditor.DesignateMusicFolder());
	}

	// Spawn cassette tapes after song
	public void SpawnCassettesFromSongs()
	{
		// Create a new list everytime this method is called
		cassettes = new List<GameObject>();

		foreach (AudioClip clip in SongManager.instance.songs)
		{
			GameObject obj = Instantiate(cassettePrefab, cassetteSpawn);

			obj.name = clip.name;
			obj.AddComponent<CassetteTape>().SetReferenceSong(clip);
			obj.AddComponent<Rigidbody>();

			cassettes.Add(obj);			
		}
	}
}
