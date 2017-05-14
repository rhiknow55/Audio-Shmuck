using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeatBoxRpgManager : MonoBehaviour {

	public MicrophoneInput micInputScript;
	public BeatBoxRpgIndicator indicatorScript;

	List<GameObject> activeRings;

	void Start() {
		activeRings = new List<GameObject>();
	}

	void Update() {
		MicDetection();
		HighLightRing();
	}

	void MicDetection() {
		/*
		if(micInputScript.CheckForFreqSpike().Count > 0) {
			CheckAccuracyOfMicInput();
		}*/
		if (Input.GetKeyDown(KeyCode.Space)) {
			CheckAccuracyOfMicInput();
		}
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
