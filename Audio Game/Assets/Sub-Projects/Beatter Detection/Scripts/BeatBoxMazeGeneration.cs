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
	public GameObject[] numbers3D = new GameObject[9];

	// Private Variables
	List<int> listOfSpikes;
	float changedTimeObstacle, changedTimeEnemy;
	int numberOfSpikesInInterval;

	int prevRandomNumpadNumber;
	Color[] colors = { Color.red, new Color(1, 165f / 255f, 0), Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta, Color.white, Color.gray };

	List<GameObject> indicatorPool, obstaclePool, enemyPool;
	Dictionary<int, List<GameObject>> numbers3DDictionary;
	List<GameObject> pool1, pool2, pool3, pool4, pool5, pool6, pool7, pool8, pool9;

	//-----------------------------------------

	void Start() {
		listOfSpikes = new List<int>();

		numbers3DDictionary = new Dictionary<int, List<GameObject>>();
		indicatorPool = new List<GameObject>();
		Init(pool1, numbers3D[0], 5, "Num 1", 1);
		Init(pool2, numbers3D[1], 5, "Num 2", 2);
		Init(pool3, numbers3D[2], 5, "Num 3", 3);
		Init(pool4, numbers3D[3], 4, "Num 4", 4);
		Init(pool5, numbers3D[4], 5, "Num 5", 5);
		Init(pool6, numbers3D[5], 6, "Num 6", 6);
		Init(pool7, numbers3D[6], 7, "Num 7", 7);
		Init(pool8, numbers3D[7], 8, "Num 8", 8);
		Init(pool9, numbers3D[8], 9, "Num 9", 9);
		Init(indicatorPool, indicatorPrefab, 10, "indicator", 0);
	}

	void Init(List<GameObject> pool, GameObject prefab, int size, string name, int key) {
		pool = new List<GameObject>();
		for (int i = 0; i < size; i++) {
			GameObject obj = Instantiate(prefab);
			obj.SetActive(false);
			obj.name = name + " : " + i;
			if (obj.GetComponent<IndicatorScript>() == null) obj.AddComponent<IndicatorScript>();
			pool.Add(obj);
		}
		numbers3DDictionary.Add(key, pool);
	}

	void Update() {
		listOfSpikes = GameObject.Find("Audio Compiler").GetComponent<AudioCompiler>().CheckForFreqSpike();

		//if (listOfSpikes.Contains(1) && Time.time >= changedTimeObstacle + spawnInterval) GenerateObstacles();
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

		GameObject obj = numbers3DDictionary[0][0];
		numbers3DDictionary[0].RemoveAt(0);

		if (obj.GetComponent<IndicatorScript>() == null) obj.AddComponent<IndicatorScript>();
		obj.GetComponent<IndicatorScript>().SetEndPos(leftEnd.position);
		obj.GetComponent<IndicatorScript>().SetStartPos(leftSpawn.position);
		obj.GetComponent<IndicatorScript>().SetTimeItTakes(timeItTakesForIndicatorToReachPlayer);
		obj.GetComponent<IndicatorScript>().ResetIndicator();
		obj.GetComponent<MeshRenderer>().material.color = Color.black;

		obj.SetActive(true);
	}

	void SpawnEnemies() {
		// Set the "random" number for the next indicator
		int rand = SetIndicatorRandomNumber();
		// Use this random number to decide which key to use for finding the correct list
		GameObject obj = numbers3DDictionary[rand][0];
		numbers3DDictionary[rand].RemoveAt(0);

		obj.GetComponent<IndicatorScript>().SetEndPos(rightEnd.position);
		obj.GetComponent<IndicatorScript>().SetStartPos(rightSpawn.position);
		obj.GetComponent<IndicatorScript>().SetTimeItTakes(timeItTakesForIndicatorToReachPlayer);
		obj.GetComponent<IndicatorScript>().ResetIndicator();
		
		obj.GetComponent<IndicatorScript>().SetRandomNumber(rand);
		obj.GetComponent<MeshRenderer>().material.color = colors[rand - 1];

		obj.SetActive(true);
	}

	/// Initially prevRandomNumpadNumber is 0, so it is chosen to be any random number from 1 to 9 inclusive.
	/// Then the next random number can only be one of the keys adjacent to the previous random number.
	/// For ex., if previous rand number was 5, next can only be 4, 8, 2, 6 ||| if prev was 7, next can be 4 or 8
	
	int SetIndicatorRandomNumber() {
		int currentRandomNumpadNumber = 0;

		switch (prevRandomNumpadNumber) {
			case 0:
				currentRandomNumpadNumber = Random.Range(1, 10);
				break;
			case 1:
				int[] arr1 = { 2, 4 };
				int rand1 = Random.Range(0, 2);
				currentRandomNumpadNumber = arr1[rand1];
				break;
			case 2:
				int[] arr2 = { 1, 3, 5};
				int rand2 = Random.Range(0, 3);
				currentRandomNumpadNumber = arr2[rand2];
				break;
			case 3:
				int[] arr3 = { 2, 6 };
				int rand3 = Random.Range(0, 2);
				currentRandomNumpadNumber = arr3[rand3];
				break;
			case 4:
				int[] arr4 = { 1, 5, 7 };
				int rand4 = Random.Range(0, 3);
				currentRandomNumpadNumber = arr4[rand4];
				break;
			case 5:
				int[] arr5 = { 2, 4, 6, 8 };
				int rand5 = Random.Range(0, 4);
				currentRandomNumpadNumber = arr5[rand5];
				break;
			case 6:
				int[] arr6 = { 3, 5, 9 };
				int rand6 = Random.Range(0, 3);
				currentRandomNumpadNumber = arr6[rand6];
				break;
			case 7:
				int[] arr7 = { 4, 8 };
				int rand7 = Random.Range(0, 2);
				currentRandomNumpadNumber = arr7[rand7];
				break;
			case 8:
				int[] arr8 = { 5, 7, 9 };
				int rand8 = Random.Range(0, 3);
				currentRandomNumpadNumber = arr8[rand8];
				break;
			case 9:
				int[] arr9 = { 6, 8 };
				int rand9 = Random.Range(0, 2);
				currentRandomNumpadNumber = arr9[rand9];
				break;
			default:
				print("Shouldn't have gotten to the default of a random number generator fom 1-9");
				break;
		}

		prevRandomNumpadNumber = currentRandomNumpadNumber;
		return currentRandomNumpadNumber;
	}

	public void ReturnIndicatorToPool(GameObject i) {
		int correspondingDictionaryKey = i.GetComponent<IndicatorScript>().GetRandomNumber();
		numbers3DDictionary[correspondingDictionaryKey].Add(i);
	}
}
