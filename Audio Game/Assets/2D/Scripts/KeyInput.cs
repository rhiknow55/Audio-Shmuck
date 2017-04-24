using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects key presses and changes the keypads to cyan.
// Pads automatically fade back to black within timeUntilFade seconds
public class KeyInput : MonoBehaviour {


	public GameObject up, down, left, right, rightpad, leftpad;
	float startTimeL, startTimeR, timeL, timeR;
	float timeUntilFade = 0.5f;

	void Start() {
		timeL = timeR = 1;
	}

	void Update() {
		// timeL is the lerp interpolation value from 0 to 1. 0 is cyan and 1+ is black
		if(startTimeL != 0) timeL = TimePassed(startTimeL) / timeUntilFade;
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			leftpad.GetComponent<MeshRenderer>().material.color = Color.cyan;
			startTimeL = Time.time;
		}
		leftpad.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.cyan, Color.black, timeL); ;
		
		if(startTimeR != 0) timeR = TimePassed(startTimeR) / timeUntilFade;
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			rightpad.GetComponent<MeshRenderer>().material.color = Color.cyan;
			startTimeR = Time.time;
		}
		rightpad.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.cyan, Color.black, timeR);
	}
	
	float TimePassed(float startTime) {
		float currentTime = Time.time;
		return currentTime - startTime;
	}
}
