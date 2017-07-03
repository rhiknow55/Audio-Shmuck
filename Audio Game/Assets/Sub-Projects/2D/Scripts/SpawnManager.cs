using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	
	public enum KEY { UP, DOWN, LEFT, RIGHT, Z, X, C };

	//public static bool UP, DOWN, LEFT, RIGHT, Z, X, C;
	const float MIN_FREQ = 1E-04f;
	public GameObject orbPrefab;
	List<GameObject> orbPool;
	bool canSpawnSB, canSpawnB, canSpawnLM;
	float stSB, stB, stLM;
	public float spawnRate, ceilingFactor;
	public static List<GameObject> subBassWallPool, bassWallPool, lowMidrangeWallPool;
	public GameObject subBassWallPrefab, bassWallPrefab, lowMidrangeWallPrefab;
	public float peakRatio;
	float[] ceilings = new float[3];
	float ceilingLargest;
	int ceilingLargestIndex;
	public static List<GameObject> activeSB, activeB;
	/*
	void Start() {
		activeSB = new List<GameObject>();
		activeB = new List<GameObject>();
		subBassWallPool = new List<GameObject>();
		bassWallPool = new List<GameObject>();
		lowMidrangeWallPool = new List<GameObject>();
		canSpawnSB = canSpawnB = canSpawnLM = true;
		Init(subBassWallPool, subBassWallPrefab, 20, "SubBassWall");
		Init(bassWallPool, bassWallPrefab, 20, "BassWall");
		Init(lowMidrangeWallPool, lowMidrangeWallPrefab, 100, "LowMidrangeWall");
	}

	void Init(List<GameObject> pool, GameObject prefab, int size, string name) {
		for (int i = 0; i < size; i++) {
			GameObject obj = Instantiate(prefab);
			obj.SetActive(false);
			obj.name = name + i;
			pool.Add(obj);
		}
	}

	void Update() {
		Ceiling();
		SpawnObstacles(0);
		
		SpawnObstacles(1);
	}

	void Ceiling() {
		// min + (max-min)/ceilingFactor
		ceilings[0] = AudioCompiler.minSubBass + (AudioCompiler.maxSubBass - AudioCompiler.minSubBass) / ceilingFactor;
		ceilings[1] = AudioCompiler.minBass + (AudioCompiler.maxBass - AudioCompiler.minBass) / ceilingFactor;
	}

	void SpawnObstacles(int index) {
		//floor prevents small sounds from triggering spawnobstacle
		float floor = AudioCompiler.freqBandHighest[index] / ceilingFactor;
		switch (index) {
			case 0:
				if (AudioCompiler.freqBands[index] > ceilings[index] && canSpawnSB && AudioCompiler.freqBands[index] > floor && AudioCompiler.freqBands[index] > MIN_FREQ) {
					stSB = Time.time;
					SpawnObstacle(subBassWallPool, index);
					canSpawnSB = false;
				}

				if (Time.time > stSB + spawnRate && AudioCompiler.freqBands[index] < ceilings[index]) {
					canSpawnSB = true;
				}
				break;
			case 1:
				if (AudioCompiler.freqBands[index] > ceilings[index] && canSpawnB && AudioCompiler.freqBands[index] > floor && AudioCompiler.freqBands[index] > MIN_FREQ) {
					stB = Time.time;
					SpawnObstacle(bassWallPool, index);
					canSpawnB = false;
				}

				if (Time.time > stB + spawnRate && AudioCompiler.freqBands[index] < ceilings[index]) {
					canSpawnB = true;
				}
				break;
		}
		
	}

	void SpawnObstacle(List<GameObject> pool, int index) {
		GameObject obj = pool[0];
		pool.RemoveAt(0);
		switch (index) {
			case 0:
				Vector3 spawnPos = new Vector3(this.transform.position.x + obj.transform.localScale.x/2, this.transform.position.y + obj.transform.localScale.y / 2, this.transform.position.z);
				obj.GetComponent<WallInterface>().SetInitialPos(spawnPos);
				Color startingColor = new Color(1, 0, 0, 0.2f);
				obj.GetComponent<WallInterface>().SetInitialColor(startingColor);
				obj.transform.rotation = this.transform.rotation;
				obj.GetComponent<WallInterface>().StartTimer();

				obj.SetActive(true);
				activeSB.Add(obj);
				break;
			case 1:
				Vector3 spawnPos1 = new Vector3(this.transform.position.x - obj.transform.localScale.x / 2, this.transform.position.y + obj.transform.localScale.y / 2, this.transform.position.z);
				obj.GetComponent<WallInterface>().SetInitialPos(spawnPos1);
				Color startingColor1 = new Color(0, 1, 0, 0.2f);
				obj.GetComponent<WallInterface>().SetInitialColor(startingColor1);
				obj.transform.rotation = this.transform.rotation;
				obj.GetComponent<WallInterface>().StartTimer();

				obj.SetActive(true);
				activeB.Add(obj);
				break;
		}
	}*/
}
