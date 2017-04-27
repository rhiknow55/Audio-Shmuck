using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In charge of generating the maze as the song progresses
/// </summary>
public class BeatBoxMazeGeneration : MonoBehaviour {

	// Public Variables
	public float clutterSpikeInterval, spawnInterval;
	public Transform rightSpawn, leftSpawn, rightEnd, leftEnd;
	public GameObject indicatorPrefab;
	public float timeItTakesForIndicatorToReachPlayer;

	// Private Variables
	List<int> listOfSpikes;
	float changedTimeObstacle, changedTimeEnemy;
	int numberOfSpikesInInterval;

	List<GameObject> indicatorPool, obstaclePool, enemyPool;

	//-----------------------------------------

	void Start() {
		listOfSpikes = new List<int>();

		indicatorPool = new List<GameObject>();
		Init(indicatorPool, indicatorPrefab, 30, "indicator");
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
		listOfSpikes = GameObject.Find("Audio Compiler").GetComponent<AudioCompiler>().CheckForFreqSpike();

		if (listOfSpikes.Contains(1) && Time.time >= changedTimeObstacle + spawnInterval) GenerateObstacles();
		if (listOfSpikes.Count > 0 && !listOfSpikes.Contains(1) && Time.time >= changedTimeEnemy + spawnInterval) GenerateEnemies();
	}

	
	void GenerateObstacles() {
		SpawnObstacles();
		changedTimeObstacle = ResetPoolToSpawn();
		changedTimeEnemy = ResetPoolToSpawn();
	}

	void GenerateEnemies() {
		SpawnEnemies();
		changedTimeEnemy = ResetPoolToSpawn();
	}

	float ResetPoolToSpawn() {
		return Time.time;
	}

	// Randomize obstacle spawn? have a system? pattern?
	void SpawnObstacles() {
		print("Obstacle spawned");

		GameObject obj = indicatorPool[0];
		indicatorPool.RemoveAt(0);

		if(obj.GetComponent<IndicatorScript>() == null) obj.AddComponent<IndicatorScript>();
		obj.GetComponent<IndicatorScript>().SetEndPos(leftEnd.position);
		obj.GetComponent<IndicatorScript>().SetStartPos(leftSpawn.position);
		obj.GetComponent<IndicatorScript>().SetTimeItTakes(timeItTakesForIndicatorToReachPlayer);
		obj.GetComponent<IndicatorScript>().ResetIndicator();

		obj.SetActive(true);
	}

	void SpawnEnemies() {
		print("Enemy spawned");

		GameObject obj = indicatorPool[0];
		indicatorPool.RemoveAt(0);

		if (obj.GetComponent<IndicatorScript>() == null) obj.AddComponent<IndicatorScript>();
		obj.GetComponent<IndicatorScript>().SetEndPos(rightEnd.position);
		obj.GetComponent<IndicatorScript>().SetStartPos(rightSpawn.position);
		obj.GetComponent<IndicatorScript>().SetTimeItTakes(timeItTakesForIndicatorToReachPlayer);
		obj.GetComponent<IndicatorScript>().ResetIndicator();

		obj.SetActive(true);
	}

	public void ReturnIndicatorToPool(GameObject i) {
		indicatorPool.Add(i);
	}
}
