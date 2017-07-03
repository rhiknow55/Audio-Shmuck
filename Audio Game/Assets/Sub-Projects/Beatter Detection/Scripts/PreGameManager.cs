using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PreGameManager : MonoBehaviour {

	public static bool calibrating;
	public static float calibratedFreq;

	public GameObject[] objectsToDisableAtMenu = new GameObject[8];
	public GameObject canvas;

	string microphone;
	AudioSource micCalibrationRecording;
	int audioSampleRate = 44100;
	List<string> options = new List<string>();
	GameObject micCalibrationObj;

	void Start() {
		for (int i = 0; i < objectsToDisableAtMenu.Length; i++) {
			objectsToDisableAtMenu[i].SetActive(false);
		}
	}

	public void StartGame() {
		for (int i = 0; i < objectsToDisableAtMenu.Length; i++) {
			objectsToDisableAtMenu[i].SetActive(true);
		}
		canvas.SetActive(false);
	}

	public void CalibrateMicrophone() {
		if (!calibrating) {
			calibrating = true;

			micCalibrationObj = new GameObject();
			micCalibrationObj.AddComponent<AudioSource>();
			micCalibrationObj.AddComponent<MicrophoneInput>();
		}
	}

	public void StopCalibration() {
		calibratedFreq = micCalibrationObj.GetComponent<MicrophoneInput>().GetHighestFreqOverall();
		calibrating = false;
		print(calibratedFreq + " ::::: " + micCalibrationObj.GetComponent<MicrophoneInput>().GetHighestFreqInstant());
		Destroy(micCalibrationObj);
	}
}
