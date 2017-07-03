using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The types of beat hit proficiency levels:
/// 1. Perfect - Really close to the size of the indicator - 1.3 to multiplier!
/// 2. Great - Either before or after the size of the indicator - 1.1 to multiplier
/// 3. Good - Same as great but further away - keep multiplier
/// 4. Early - No points, ruins multiplier
/// 5. Late - No points, ruins multipliser
/// </summary>
/// 
public class RingScript : MonoBehaviour {

	BeatBoxRpgIndicator indicatorScript;

	int proficientyLevel;
	float initialScale;
	GameObject connectedIndicator;

	void Start() {
		proficientyLevel = 1;
	}

	// Set proficiency level depending on the scale of the ring!
	void Update() {
		SetProficiencyLevel();
	}






	
	void SetProficiencyLevel() {
		if (initialScale == 0) return;


		float currentScale = transform.localScale.x;
		float goodOver = (initialScale - 1f) * 0.30f + 1f; // 30% of initialScale
		float greatOver = (initialScale - 1f) * 0.12f + 1f; // 12% of initialScale
		float perfectOver = (initialScale - 1f) * 0.03f + 1f; // 3% of initialScale
		float perfectUnder = 1f - (initialScale - 1f) * 0.03f; // 3% under 1f
		float greatUnder = 1f - (initialScale - 1f) * 0.06f; // -6%
		float goodUnder = 1f - (initialScale - 1f) * 0.12f; // -12%

		// Perfect - [3%, -3%]
		if (currentScale <= perfectOver && currentScale >= perfectUnder) proficientyLevel = 1;

		// Great - [12%, 3%) and (-3%, -6%]
		if (currentScale <= greatOver && currentScale > perfectOver) proficientyLevel = 2;
		if (currentScale < perfectUnder && currentScale >= greatUnder) proficientyLevel = 2;

		// Good - [30%, 12%) and (-6%, -12%]
		if (currentScale <= goodOver && currentScale > greatOver) proficientyLevel = 3;
		if (currentScale < greatUnder && currentScale >= goodUnder) proficientyLevel = 3;

		// Early - [100%, 30%)
		if (currentScale <= initialScale && currentScale > goodOver) proficientyLevel = 4;

		// Late - (-12%, -100%]
		if (currentScale < goodUnder && currentScale >= 0.0f) {
			proficientyLevel = 5;
			indicatorScript.ReturnToPool(this.gameObject);
		}

		
	}
	
	public void SetInitialScale(float s) {
		initialScale = s;
	}

	public int GetProficiencyLevel() {
		return proficientyLevel;
	}

	public void SetIndicatorScript(BeatBoxRpgIndicator script) {
		indicatorScript = script;
	}
	/*
	public void SetReferenceToIndicator(GameObject ind) {
		connectedIndicator = ind;
	}

	public GameObject ReturnConnectedIndicator() {
		return connectedIndicator;
	}*/
}
