using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeatBoxRpgManager : MonoBehaviour {
	
	public GameObject micInput;
	public BeatBoxRpgIndicator indicatorScript;
	public bool useMicDetection;

	List<GameObject> activeRings;
	bool micBeatDetectionReset;
	float timestampOfMicBeatDetection;
	float beatDetectionDelay = 0.2f;
	float freqAccuracyThresholdPercentage = 0.8f;
	float lowerThreshold;
	float highestMicInputFreq;

	void Start() {
		activeRings = new List<GameObject>();
		micBeatDetectionReset = true;
		lowerThreshold = PreGameManager.calibratedFreq - PreGameManager.calibratedFreq * freqAccuracyThresholdPercentage;
		print("Lower Threshold : " + lowerThreshold);
	}

	void Update() {
		BeatDetection();
		HighLightRing();
	}

	void BeatDetection() {
		if (useMicDetection) {
			MicDetection();
		}
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			CheckAccuracyOfMicInput();
		}
	}

	void MicDetection() {
		if (micBeatDetectionReset) {
			highestMicInputFreq = micInput.GetComponent<MicrophoneInput>().GetHighestFreqInstant();

			
			// If mic input highest freq is similar to what was originally calibrated, its fine
			if (highestMicInputFreq >= lowerThreshold) {
				print("highest : " + highestMicInputFreq);
				CheckAccuracyOfMicInput();
			}

			micBeatDetectionReset = false;
			timestampOfMicBeatDetection = Time.time;
		}
		if (micInput.GetComponent<MicrophoneInput>().CheckForFreqSpike().Count == 0 && Time.time >= timestampOfMicBeatDetection + beatDetectionDelay) micBeatDetectionReset = true;
	}


	void HighLightRing() {
		for (int i = 0; i < activeRings.Count; i++) {
			GameObject currentRing = activeRings[i];
			if(i == 0) currentRing.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
			else currentRing.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
		}
			
		
	}


	void CheckAccuracyOfMicInput() {
		if (activeRings.Count == 0) {
			WrongBeat();
			return;
		}
		GameObject currentRing = activeRings[0];

		switch (currentRing.GetComponent<RingScript>().GetProficiencyLevel()) {
			case 1:
				PerfectBeat();
				break;
			case 2:
				GreatBeat();
				break;
			case 3:
				GoodBeat();
				break;
			case 4:
				EarlyBeat();
				break;
			case 5:
				LateBeat();
				break;
			default:
				Debug.Log("Should NOT get this switch result!");
				break;
		}

		indicatorScript.ReturnToPool(currentRing);
	}

	void PerfectBeat() {
		print("perfect");
	}

	void GreatBeat() {
		print("great");
	}

	void GoodBeat() {
		print("good");
	}

	void EarlyBeat() {
		print("early");
	}

	void LateBeat() {
		print("late");
	}


	void WrongBeat() {

	}












	public bool ContainsThisObject(GameObject o) {
		string s1 = o.name;
		
		for(int i = 0; i < activeRings.Count; i++) {
			string s2 = activeRings[i].name;
			if (s2.GetHashCode().Equals(s1.GetHashCode())) {
				return true;
			}
		}
		return false;
	}

	public void AddActiveRing(GameObject o) {
		//print("Added to manager : " + o.name);
		activeRings.Add(o);
	}

	public void RemoveRing() {
		//print("Removed from manager : " + activeRings[0].name);
		activeRings.RemoveAt(0);
	}
}
