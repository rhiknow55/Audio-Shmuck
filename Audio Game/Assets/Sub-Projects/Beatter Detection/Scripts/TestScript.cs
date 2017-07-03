using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	public bool useMicInput;
	public int index;
	public Color color;
	float changedTime;
	List<int> list;

	void Update() {
		if(Time.time >= changedTime +0.1f) this.GetComponent<MeshRenderer>().material.color = Color.white;
		if(!useMicInput) list = GameObject.Find("Audio Compiler").GetComponent<AudioCompiler>().CheckForFreqSpike();
		if(useMicInput) list = GameObject.Find("Microphone Input").GetComponent<MicrophoneInput>().CheckForFreqSpike();
		if (list.Contains(index)) {
			this.GetComponent<MeshRenderer>().material.color = color;
			changedTime = Time.time;
		}
	}
}
