using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowMidrange2D : MonoBehaviour {
	public int midRangeFreqIndex;
	Vector3 scaleVector;
	public float scaleMultiplier, startScale;
	public float emissionFactorFrom0To1;

	void Update() {
		Scale();
	}

	void Scale() {
		scaleVector = new Vector3(transform.localScale.x, (AudioPlayer.lowMidrangeFreqBands[midRangeFreqIndex] * scaleMultiplier + startScale), transform.localScale.z);
		transform.localScale = scaleVector;
		//adjust position accordingly to scale
		//transform.localPosition = new Vector3(transform.position.x, ((AudioPlayer.lowMidrangeFreqBands[midRangeFreqIndex] * scaleMultiplier) + startScale) / 2, transform.position.z);
		
	}
}
