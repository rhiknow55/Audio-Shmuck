using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Spawn 20 audiovisualizer prefabs for both AudioCompiler and MicrophoneInput on the corresponding sides
/// - Update their z scale upon getting input
/// - Move z position to accomodate z scale change
/// 
/// </summary>

public class Manager : MonoBehaviour {

	// Public Variables
	public float scaleMultiplier;
	public int numberOfVisualizers;
	public GameObject audioVisualizerPrefab;
	public float startScale;

	// Private Variables
	float initialZPos, initialYPos, initialXPos;

	void Start() {
		initialXPos = 105;
		initialYPos = 0.5f;
		initialZPos = 50f;
		InitAudioCompilerObjects();
		InitMicrophoneInputObjects();
		
	}

	void InitAudioCompilerObjects() {
		GameObject parent = new GameObject("AudioCompilerObjects");
		for (int i = 0; i < numberOfVisualizers; i++) {
			GameObject obj = Instantiate(audioVisualizerPrefab);
			float zPos = initialZPos - audioVisualizerPrefab.transform.localScale.z * i - audioVisualizerPrefab.transform.localScale.z / 2;
			obj.transform.position = new Vector3(-initialXPos, initialYPos, zPos);
			obj.AddComponent<AudioVisualizer>();
			obj.name = "AudioCompiler" + i;
			obj.GetComponent<AudioVisualizer>().Initialize(scaleMultiplier, i, 0, startScale);

			obj.transform.parent = parent.transform;
		}
	}

	void InitMicrophoneInputObjects() {
		GameObject parent = new GameObject("MicrophoneInputObjects");
		for (int i = 0; i < numberOfVisualizers; i++) {
			GameObject obj = Instantiate(audioVisualizerPrefab);
			float zPos = initialZPos - audioVisualizerPrefab.transform.localScale.z * i - audioVisualizerPrefab.transform.localScale.z / 2;
			obj.transform.position = new Vector3(initialXPos, initialYPos, zPos);
			obj.AddComponent<AudioVisualizer>();
			obj.name = "MicrophoneInput" + i;
			obj.GetComponent<AudioVisualizer>().Initialize(scaleMultiplier, i, 1, startScale);

			obj.transform.parent = parent.transform;
		}
	}

	void Update() {

	}
}
