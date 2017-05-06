using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour {

	// Public Variables
	

	// Private Variables
	Vector3 startPos, endPos;
	float traversalTime;
	float timeItTakesToReachEndPos;
	float startTime;
	float leewayTime = 0.3f;
	int randomNumpadNumber;
	bool insideTrigger;

	void Update() {
		// leewayTime is for the player to be able to hit the beat for some time AFTER the actual beat - in case the player reacts late
		if (this.transform.position == endPos && TimePassed() > timeItTakesToReachEndPos + leewayTime) {
			BackToPool();
		}else {
			traversalTime = TimePassed() / timeItTakesToReachEndPos;
			PosLerp();
		}
	}

	//------------------------------------------------
	public void OnTriggerEnter(Collider col) {
		if (col.tag == "GreenTrigger") {
			insideTrigger = true;
			PlayerInputDetection.triggeredIndicators.Add(this.gameObject);
		}
		
		if(col.tag == "Indicator") {
			col.GetComponent<IndicatorScript>().BackToPool();
			
		}
	}

	public bool GetIfInsideTrigger() {
		return insideTrigger;
	}
	//-------------------------------------------------

	public float TimePassed() {
		float currentTime = Time.time;
		return currentTime - startTime;
	}

	public void PosLerp() {
		this.gameObject.transform.position = Vector3.Lerp(startPos, endPos, traversalTime);
	}

	public void SetStartPos(Vector3 s) {
		startPos = s;
		this.gameObject.transform.position = startPos;
	}

	public void SetEndPos(Vector3 s) {
		endPos = s;
	}

	public void SetTimeItTakes(float t) {
		timeItTakesToReachEndPos = t;
	}

	public void ResetIndicator() {
		startTime = Time.time;
	}

	// Setting the indicator's random numpad number
	public void SetRandomNumber(int rand) {
		randomNumpadNumber = rand;
	}
	public int GetRandomNumber() {
		return randomNumpadNumber;
	}

	public void BackToPool() {
		this.gameObject.SetActive(false);
		insideTrigger = false;
		if(PlayerInputDetection.triggeredIndicators.Count > 0) PlayerInputDetection.triggeredIndicators.RemoveAt(0);
		GameObject.Find("BeatBoxMazeGenerator").GetComponent<BeatBoxMazeGeneration>().ReturnIndicatorToPool(this.gameObject);
	}
}
