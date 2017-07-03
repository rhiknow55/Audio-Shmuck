using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour {

	private int indexForWhichAudio;
	private float scaleMultiplier;
	private int freqIndex;
	private float startScale;
	private Vector3 initialPos;

	public void Initialize(float scaleMultiplier, int freqIndex, int indexForWhichAudio, float startScale) {
		this.scaleMultiplier = scaleMultiplier;
		this.freqIndex = freqIndex;
		this.indexForWhichAudio = indexForWhichAudio;
		this.startScale = startScale;
	}

	void Start() {
		initialPos = transform.localPosition;
	}

	void Update() {
		ScaleAndReposition();
		//print(freqIndex + " ::: "+AudioCompiler.freqSubbandsInstant[freqIndex]);
	}

	void ScaleAndReposition() {
		switch (indexForWhichAudio) {
			case 0:
				float x0 = scaleMultiplier * AudioCompiler.freqSubbandsInstant[freqIndex] + startScale;
				transform.localScale = new Vector3(x0, transform.localScale.y, transform.localScale.z);

				float p0 = initialPos.x + x0 / 2;
				transform.localPosition = new Vector3(p0, transform.localPosition.y, transform.localPosition.z);
				break;
			case 1:
				float x1 = scaleMultiplier * MicrophoneInput.freqSubbandsInstant[freqIndex] + startScale;
				transform.localScale = new Vector3(x1, transform.localScale.y, transform.localScale.z);

				float p1 = initialPos.x - x1 / 2;
				transform.localPosition = new Vector3(p1, transform.localPosition.y, transform.localPosition.z);
				break;
			default:
				print("You fucked up somewhere!");
				break;
		}
	}
}
