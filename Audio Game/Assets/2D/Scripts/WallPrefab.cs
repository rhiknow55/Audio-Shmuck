using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WallPrefab : MonoBehaviour {

	public bool keyPressedOnTime, firstPress, keyPressed;

	void OnTriggerEnter(Collider col) {
		if (col.tag == "despawn") {
			Debug.Log("yo");
			BackToPool();
		}

		if (col.tag == "GreenTrigger") {
			if (keyPressed) {
				keyPressedOnTime = true;
				ScoreManager.AddScore(100);
			}
		}

		if (col.tag == "YellowTrigger") {
			if (keyPressed) {
				keyPressedOnTime = true;
				ScoreManager.AddScore(50);
			}
		}
	}

	public abstract void BackToPool();
}
