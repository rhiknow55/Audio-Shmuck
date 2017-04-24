using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public bool spawnOnRight;
	List<GameObject> pool;
	public int poolSize;
	public GameObject enemyPrefab;
	public float depthRange;

	void Start(){
		pool = new List<GameObject>();
		for(int i = 0; i < poolSize; i++){
			GameObject instantiatedEnemy = Instantiate(enemyPrefab);
			instantiatedEnemy.name = "Enemy"+i;
			instantiatedEnemy.SetActive(false);
			instantiatedEnemy.transform.position = this.gameObject.transform.position;
			pool.Add(instantiatedEnemy);
		}
	}
	/*
	void Update() {
		Spawn();
	}

	void Spawn(){
		if(AudioCompiler.freqBands[0] == 1){
			TakeFromPool();
		}
	}*/

	void TakeFromPool() {
		GameObject enemy = pool[0];
		pool.RemoveAt(0);
		float depth = Random.Range(-1f, 1f) * depthRange;
		Debug.Log(depth);
		Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y,  depth);
		enemy.gameObject.transform.position = spawnPos;
		//enemy.transform.LookAt(2 * orb.transform.position - this.transform.position);
		Debug.Log("Enemy" + " position is = " +enemy.transform.position);
		enemy.SetActive(true);

	}
}
