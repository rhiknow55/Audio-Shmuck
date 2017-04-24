using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatOrb : MonoBehaviour {
	
	float startTime;
	Color initialColor, lerpingColor;
	Vector3 initialPos, finalPos;
	public float finalPosZ;

	void Start() {
		finalPos = new Vector3(initialPos.x, initialPos.y, finalPosZ);
	}

	void Update() {
		ColorLerp();
		PosLerp();
	}

	void ColorLerp() {
		lerpingColor = Color.Lerp(initialColor, Color.white, timePassed() / 3);
		this.GetComponent<MeshRenderer>().material.color = lerpingColor;
	}

	void PosLerp() {
		this.gameObject.transform.position = Vector3.Lerp(initialPos, finalPos, timePassed() / 3);
	}

	public void setInitialPos(Vector3 startingPos) { initialPos = startingPos; }

	public void setInitialColor(Color startingColor) { initialColor = startingColor; }

	public void startTimer() {
		startTime = Time.time;
	}

	public float timePassed() {
		float currentTime = Time.time;
		return currentTime - startTime;
	}
}
