using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBoxRpgIndicator : MonoBehaviour {

	// Public Variables
	public GameObject ringPrefab;
	public float spawnInterval;
	public float initialSizeOfRingWhenItSpawns;
	public Transform initialRingTransform, destinationTransform;
	public BeatBoxRpgManager managerScript;
	public AudioCompiler audioCompilerScript;

	// Private Variables
	float leeway;
	List<GameObject> ringPool;
	List<int> listOfSpikes;
	float spawnedTimeBass, spawnedTimeRing;
	Vector3 initialScale;
	float yScale;

	void Start() {
		yScale = ringPrefab.transform.localScale.y;
		initialScale = ringPrefab.transform.localScale;
		ringPool = new List<GameObject>();
		Init(ringPool, ringPrefab, 20, "Ring");
	}

	void Init(List<GameObject> pool, GameObject prefab, int size, string name) {
		
		for (int i = 0; i < size; i++) {
			GameObject obj = Instantiate(prefab);
			obj.SetActive(false);
			obj.name = name + " : " + i;
			if (obj.GetComponent<RingScript>() == null) obj.AddComponent<RingScript>();
			obj.GetComponent<RingScript>().SetIndicatorScript(this);
			pool.Add(obj);
		}
	}

	void Update() {
		listOfSpikes = audioCompilerScript.CheckForFreqSpike();

		//if (listOfSpikes.Contains(1) && Time.time >= spawnedTimeBass + spawnInterval) GenerateBass();
		if (listOfSpikes.Count > 0 && !listOfSpikes.Contains(1) && Time.time >= spawnedTimeRing + spawnInterval) GenerateRing();

	}

	void GenerateBass() {
		SpawnBass();
		spawnedTimeBass = CurrentTime();
		spawnedTimeRing = CurrentTime();
	}

	void GenerateRing() {
		SpawnRing();
		spawnedTimeRing = CurrentTime();
	}

	float CurrentTime() {
		return Time.time;
	}

	// Spawns larger rectangle for the bass beats
	void SpawnBass() {

	}

	// Spawns rings slightly behind indicator, and shrinks the ring down
	// Make it spawn randomly!
	void SpawnRing() {
		
		GameObject obj = ringPool[0];
		ringPool.RemoveAt(0);
		

		// Spawns the ring slightly behind the ring indicator
		obj.transform.position = initialRingTransform.position;
		obj.transform.localScale = initialScale * initialSizeOfRingWhenItSpawns;
		obj.GetComponent<RingScript>().SetInitialScale(initialSizeOfRingWhenItSpawns);
		obj.SetActive(true);


		managerScript.AddActiveRing(obj);

		StartCoroutine("ScaleObject", obj);
		StartCoroutine("MoveObject", obj);
	}

	public void ReturnToPool(GameObject obj) {
		obj.transform.localScale = initialScale * initialSizeOfRingWhenItSpawns;
		obj.SetActive(false);
		ringPool.Add(obj);

		//remove this object from the actives pool in the manager
		managerScript.RemoveRing();
	}

	Vector2 RandomXAndYAxis() {
		float x = Random.Range(1f, 9f);
		float y = Random.Range(1f, 9f);
		return new Vector2(x, y);
	}

	IEnumerator ScaleObject(GameObject obj) {
		float scaleDuration = initialSizeOfRingWhenItSpawns;                                //animation duration in seconds
		Vector3 actualScale = obj.transform.localScale;             // scale of the object at the begining of the animation
		Vector3 targetScale = new Vector3(1f, yScale, 1f);     // scale of the object at the end of the animation

		for (float t = 0; t < 1; t += Time.deltaTime / scaleDuration) {
			obj.transform.localScale = Vector3.Lerp(actualScale, targetScale, t);
			yield return null;
		}
		
	}

	IEnumerator MoveObject(GameObject obj) {
		float duration = initialSizeOfRingWhenItSpawns;
		Vector3 initialPos = obj.transform.position;
		Vector3 endPos = destinationTransform.position;

		for (float t = 0; t < 1; t += Time.deltaTime / duration) {
			obj.transform.position = Vector3.Lerp(initialPos, endPos, t);
			yield return null;
		}
		// Make the ring shrink down and fade away
		//print(managerScript.ContainsThisObject(obj));
		//if(managerScript.ContainsThisObject(obj)) 
		StartCoroutine("FadeOutObject", obj);
	}

	IEnumerator FadeOutObject(GameObject obj) {
		float duration = initialSizeOfRingWhenItSpawns / (initialSizeOfRingWhenItSpawns - 1f);
		Vector3 actualScale = obj.transform.localScale;             // scale of the object at the begining of the animation
		Vector3 targetScale = new Vector3(0f, yScale, 0f);

		for (float t = 0; t < 1; t += Time.deltaTime / duration) {
			obj.transform.localScale = Vector3.Lerp(actualScale, targetScale, t);
			yield return null;
		}
		
	}
}
