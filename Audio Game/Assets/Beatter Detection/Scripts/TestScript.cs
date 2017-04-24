using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	public int index;
	public Color color;
	float changedTime;

	void Update() {
		if(Time.time >= changedTime +0.1f) this.GetComponent<MeshRenderer>().material.color = Color.white;
		List<int> list = GameObject.Find("Audio Compiler").GetComponent<AudioCompiler>().CheckForFreqSpike();
		if (list.Contains(index)) {
			this.GetComponent<MeshRenderer>().material.color = color;
			changedTime = Time.time;
		}
	}
}
