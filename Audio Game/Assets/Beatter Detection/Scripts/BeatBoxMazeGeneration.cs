using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In charge of generating the maze as the song progresses
/// </summary>
public class BeatBoxMazeGeneration : MonoBehaviour {

	// Public Variables
	public float clutterSpikeInterval, spawnInterval;

	// Private Variables
	List<int> listOfSpikes;
	float changedTimeObstacle, changedTimeEnemy;
	int numberOfSpikesInInterval;

	//-----------------------------------------

	void Start() {
		listOfSpikes = new List<int>();		
	}

	void Update() {
		listOfSpikes = GameObject.Find("Audio Compiler").GetComponent<AudioCompiler>().CheckForFreqSpike();

		if (listOfSpikes.Contains(1) && Time.time >= changedTimeObstacle + spawnInterval) DoBass();
		if (listOfSpikes.Count > 0 && !listOfSpikes.Contains(1) && Time.time >= changedTimeEnemy + spawnInterval) DoRest();
	}

	
	void DoBass() {
		DoBassMore();
		changedTimeObstacle = ResetPoolToSpawn();
	}

	void DoRest() {
		DoRestMore();
		changedTimeEnemy = ResetPoolToSpawn();
	}

	float ResetPoolToSpawn() {
		return Time.time;
	}

	// Randomize obstacle spawn? have a system? pattern?
	void DoBassMore() {
		print("Obstacle spawned");
	}

	void DoRestMore() {
		print("Enemy spawned");
	}
}
