using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {

	public static float playDelay = 3;


	AudioSource audioSource;
	private float[] samplesLeft = new float[1024];
	private float[] samplesRight = new float[1024];
	public static float[] freqBands = new float[7];

	public static float[] lowMidrangeFreqBands = new float[13];

	void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	void Update() {
		GetSpectrumAudioSource();
		MakeFrequencyBandsManual();
	}

	void GetSpectrumAudioSource() {
		audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.BlackmanHarris);
		audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.BlackmanHarris);
	}

	void MakeFrequencyBandsManual() {
		// 22050 / 1024 == 21.5hertz per sample

		// Sub Bass -- 21.5 -> 64.5
		float average = 0;
		for (int a = 1; a <= 3; a++) {
			average += samplesLeft[a];
			average += samplesRight[a];
		}
		average /= (3) * 2;
		freqBands[0] = average * 10;

		// Bass -- 64.5 -> 258
		average = 0;
		for (int b = 3; b <= 12; b++) {
			average += samplesLeft[b];
			average += samplesRight[b];
		}
		average /= (10) * 2;
		freqBands[1] = average * 10;

		// Low Midrange -- 258 -> 516
		average = 0;
		for (int c = 12; c <= 24; c++) {
			average += samplesLeft[c];
			average += samplesRight[c];
			lowMidrangeFreqBands[c - 12] = (samplesLeft[c] + samplesRight[c]) * 5;
		}
		average /= (13) * 2;
		freqBands[2] = average * 10;

		// Midrange -- 516 -> 1999.5
		average = 0;
		for (int d = 24; d <= 93; d++) {
			average += samplesLeft[d];
			average += samplesRight[d];
		}
		average /= ((93 - 24) + 1) * 2;
		freqBands[3] = average * 10;

		// Upper Midrange -- 1999.5 -> 3999
		average = 0;
		for (int e = 93; e <= 186; e++) {
			average += samplesLeft[e];
			average += samplesRight[e];
		}
		average /= (94) * 2;
		freqBands[4] = average * 10;

		// Presence -- 3999 -> 5998.5
		average = 0;
		for (int f = 186; f <= 279; f++) {
			average += samplesLeft[f];
			average += samplesRight[f];
		}
		average /= (94) * 2;
		freqBands[5] = average * 10;

		// Presence -- 5998.5 -> 22050
		average = 0;
		for (int g = 279; g <= 1023; g++) {
			average += samplesLeft[g];
			average += samplesRight[g];
		}
		average /= ((1024 - 279) + 1) * 2;
		freqBands[6] = average * 10;
	}
}
