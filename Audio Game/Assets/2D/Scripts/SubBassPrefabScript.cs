using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBassPrefabScript : MonoBehaviour, WallInterface {

	float startTime;
	Color initialColor, lerpingColor;
	Vector3 initialPos, finalPos;
	public float amountToMove;
	float despawn, despawnTime, traversalTime;
	public float scaleMultiplier, colorMultiplier;
	float startScale, startYPos;
	public bool keyPressedOnTime, firstPress, keyPressed, removed, triggered;
	int tick = 0;

	void Start() {
		finalPos = new Vector3(initialPos.x, initialPos.y, initialPos.z - amountToMove + this.transform.localScale.z/2);
		despawnTime = AudioPlayer.playDelay * 
			(GameObject.Find("Tunnel").transform.localScale.z - amountToMove) / GameObject.Find("Tunnel").transform.localScale.z;

		startScale = transform.localScale.y;
		startYPos = transform.position.y;
		firstPress = true;
	}

	void Update() {
		Scale();
		if (SpawnManager.activeSB.Count > 0) 
			if (SpawnManager.activeSB[0] == this.gameObject) Tick();

		RemoveFromActiveListIfPastAllTriggers();

		if (transform.position.z <= finalPos.z) {
			traversalTime = (TimePassed() - AudioPlayer.playDelay) / despawnTime;
			PosLerpDespawn();
			
			//if key is pressed while obj is inside collider
			if (keyPressedOnTime) ColorLerp();
		} else {
			traversalTime = TimePassed() / AudioPlayer.playDelay;
			PosLerp();
			ColorLerp();
		}
	}

	void RemoveFromActiveListIfPastAllTriggers() {
		// a coordinate where the wall edge would be leaving the trigger edge
		float edgeOfWallLeavesTrigger = finalPos.z - 50 - this.transform.localScale.z / 2;
		if (!removed && transform.position.z <= edgeOfWallLeavesTrigger) {
			if (!triggered) ScoreManager.multiplier = 1f;
			SpawnManager.activeSB.RemoveAt(0);
			removed = true;
		}
	}

	void Tick() {
		if (tick > 1) keyPressed = false;
		if (firstPress && Input.GetKeyDown(KeyCode.RightArrow)) {
			keyPressed = true;
			firstPress = false;
			tick = 0;
		}
		tick++;
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "despawn") {
			BackToPool();
		}

		if (col.tag == "Trigger") {
			if (keyPressed && !triggered) {
				keyPressedOnTime = true;
				triggered = true;
				int score = Mathf.FloorToInt(AudioPlayer.freqBands[0] * 100 * ScoreManager.multiplier);
				ScoreManager.multiplier += 0.1f;
				ScoreManager.AddScore(score);
			}
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "Trigger") {
			if (keyPressed && !triggered) {
				keyPressedOnTime = true;
				triggered = true;
				int score = Mathf.FloorToInt(AudioPlayer.freqBands[0] * 100 * ScoreManager.multiplier);
				ScoreManager.multiplier += 0.1f;
				ScoreManager.AddScore(score);
			}
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.tag == "Trigger") {
			if (keyPressed && !triggered) {
				keyPressedOnTime = true;
				triggered = true;
				int score = Mathf.FloorToInt(AudioPlayer.freqBands[0] * 100 * ScoreManager.multiplier);
				ScoreManager.multiplier += 0.1f;
				ScoreManager.AddScore(score);
			}
		}
	}

	void Scale() {
		Vector3 scaleVector = new Vector3(transform.localScale.x, (AudioPlayer.freqBands[0] * scaleMultiplier + startScale), transform.localScale.z);
		transform.localScale = scaleVector;
		//adjust position accordingly to scale
		Vector3 posVector = new Vector3(transform.position.x, (AudioPlayer.freqBands[0] * scaleMultiplier + startYPos)/2, transform.position.z);
		transform.position = posVector;
	}

	public void ColorLerp() {
		lerpingColor = Color.Lerp(initialColor, new Color(initialColor.r/ colorMultiplier, initialColor.g / colorMultiplier, initialColor.b / colorMultiplier), traversalTime);
		this.GetComponent<MeshRenderer>().material.color = lerpingColor;
	}

	public void PosLerp() {
		this.gameObject.transform.position = Vector3.Lerp(initialPos, finalPos, traversalTime);
	}

	public void PosLerpDespawn() {
		Vector3 newFinalPos = new Vector3(finalPos.x, finalPos.y, GameObject.Find("despawnCollider").transform.position.z);
		this.gameObject.transform.position = Vector3.Lerp(finalPos, newFinalPos, traversalTime);
	}

	public void SetInitialPos(Vector3 startingPos) {
		initialPos = startingPos;
		this.gameObject.transform.position = initialPos;
	}

	public void SetInitialColor(Color startingColor) { initialColor = startingColor; }

	public void StartTimer() {
		startTime = Time.time;
	}

	public float TimePassed() {
		float currentTime = Time.time;
		return currentTime - startTime;
	}

	void BackToPool() {
		this.gameObject.transform.position = initialPos;
		keyPressedOnTime = false;
		firstPress = true;
		removed = false;
		triggered = false;
		this.gameObject.SetActive(false);
		SpawnManager.subBassWallPool.Add(this.gameObject);
	}
}
