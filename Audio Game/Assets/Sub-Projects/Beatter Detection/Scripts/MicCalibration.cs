using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicCalibration : MonoBehaviour {

	// Private Lists
	private float[] samplesLeft = new float[1024];
	private float[] samplesRight = new float[1024];
	private float[] samplesStereo = new float[1024];
	private float[][] freqSubbandsAverageLocal;
	private float[] variances;
	private float[] constantC;

	public static float[] freqSubbandsInstant;

	// Public Variables


	public FFTWindow fftWindow;
	float varianceLimit = 0.08f;

	// Private Variables
	string microphone;
	int audioSampleRate = 44100;
	int numberOfSubbands = 32;

	private List<string> options = new List<string>();
	private int numberOfSamples = 1024;
	private AudioSource audioSource;

	int averageLocalIndex;
	int count = 1;


	void Start() {
		freqSubbandsInstant = new float[numberOfSubbands];
		// Instantiate a multidimensional array like this - each subband has 43 values in one second
		freqSubbandsAverageLocal = new float[numberOfSubbands][];
		for (int i = 0; i < numberOfSubbands; i++) {
			freqSubbandsAverageLocal[i] = new float[43];
		}
		// Instantiate the list of variances of the subbands
		variances = new float[numberOfSubbands];
		constantC = new float[numberOfSubbands];

		//get components you'll need
		audioSource = GetComponent<AudioSource>();

		// get all available microphones
		foreach (string device in Microphone.devices) {
			if (microphone == null) {
				//set default mic to first mic found.
				microphone = device;
			}
			options.Add(device);
		}

		UpdateMicrophone();
	}

	void UpdateMicrophone() {
		audioSource.Stop();
		//Start recording to audioclip from the mic
		audioSource.clip = Microphone.Start(microphone, true, 10, audioSampleRate);
		audioSource.loop = true;
		// Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
		//Debug.Log(Microphone.IsRecording(microphone).ToString());

		if (Microphone.IsRecording(microphone)) { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
			while (!(Microphone.GetPosition(microphone) > 0)) {
			} // Wait until the recording has started. 

			Debug.Log("recording started with " + microphone.ToString());

			audioSource.Play();
		} else {
			Debug.Log(microphone + " doesn't work!");
		}
	}

	void Update() {
		GetSpectrumAudioSource();
		CreateStereoSampleList();
		CreateSubbands();
		CalculateVarianceOfSubbands();

		GetHighestFreqInListOfFreqSpikes();
		// For calibration
		GetHighestFreqInstant();
		if (PreGameManager.calibrating) GetHighestFreqOverall();
	}

	// Step #1
	void GetSpectrumAudioSource() {
		audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.BlackmanHarris);
		audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.BlackmanHarris);
	}

	// Step #2
	void CreateStereoSampleList() {
		for (int i = 0; i < samplesStereo.Length; i++) {
			samplesStereo[i] = samplesLeft[i] + samplesRight[i];
		}
	}

	// Step #3
	void CreateSubbands() {
		// After testing out numberOfSubbands of 32, 64 and 128, 1/2 is the closest to a sum of 1024
		int count = 0;
		int logBaseToGetClosestTo1024 = numberOfSubbands / 2;
		for (int i = 1; i <= numberOfSubbands; i++) {
			int numberOfSamplesToAddToSubband = (int)(Mathf.Log(i, logBaseToGetClosestTo1024) * (numberOfSamples / numberOfSubbands)) + 1; // Reason I add 1, is b/c otherwise log(1) is null and causes errors
			float average = 0;
			// On the last subband, add the rest of sample frequencies
			if (i == numberOfSubbands) {
				int leftover = numberOfSubbands - count;
				numberOfSamplesToAddToSubband = leftover;
			}

			for (int j = 0; j < numberOfSamplesToAddToSubband; j++) {
				average += samplesStereo[count];
				count++;
			}
			average /= numberOfSamplesToAddToSubband;
			freqSubbandsInstant[i - 1] = average * 10;
		}

		SaveOneSecondOfSoundEnergy();
	}

	// Step #4
	void SaveOneSecondOfSoundEnergy() {
		for (int i = 0; i < freqSubbandsInstant.Length; i++) {
			freqSubbandsAverageLocal[i][averageLocalIndex] = freqSubbandsInstant[i];
		}
		if (averageLocalIndex < 43 - 1) averageLocalIndex++;
		else averageLocalIndex = 0;
	}

	// Step #5
	public List<int> CheckForFreqSpike() {
		List<int> spikeList = new List<int>();
		for (int i = 0; i < numberOfSubbands; i++) {
			float localAverage = 0;
			for (int j = 0; j < 43; j++) {
				localAverage += freqSubbandsAverageLocal[i][j];
			}
			localAverage /= 43;

			if (freqSubbandsInstant[i] > localAverage * constantC[i] && variances[i] > varianceLimit) {
				//Debug.Log("localAverage = " + localAverage * C + " ||||| instant = " + freqSubbandsInstant[i]);
				spikeList.Add(i);
			}
		}

		return spikeList;
	}

	// Step #6
	void CalculateVarianceOfSubbands() {
		for (int i = 0; i < variances.Length; i++) {
			float localAverage = 0;
			for (int j = 0; j < 43; j++) {
				localAverage += freqSubbandsAverageLocal[i][j];
			}
			localAverage /= 43;
			ChangeC(i, localAverage);
			variances[i] = freqSubbandsInstant[i] - localAverage;
			//Debug.Log("subband = " + i + " ||| variance of = " + variances[i]);
		}
	}

	void ChangeC(int index, float variance) {
		float newC = (variance * 0.0025714f) + 1.5142857f;
		constantC[index] = newC;
	}

	public float GetAveragedVolume() {
		float[] data = new float[256];
		float a = 0;
		audioSource.GetOutputData(data, 0);
		foreach (float s in data) {
			a += Mathf.Abs(s);
		}

		float averageVolume = a / 256 * 100;
		return averageVolume;
	}

	public float GetHighestFreqInListOfFreqSpikes() {
		List<int> spikes = CheckForFreqSpike();
		float highestFreq = 0f;

		if (spikes.Count > 0) {
			for (int i = 0; i < spikes.Count; i++) {
				if (freqSubbandsInstant[spikes[i]] > highestFreq) highestFreq = freqSubbandsInstant[spikes[i]];
			}
		}

		return highestFreq;
	}

	public float GetHighestFreqInstant() {
		float highestFreq = 0f;

		for (int i = 0; i < freqSubbandsInstant.Length; i++) {
			if (freqSubbandsInstant[i] > highestFreq) highestFreq = freqSubbandsInstant[i];
		}

		return highestFreq;
	}

	float highestFreqOverall = 0;
	// For calibration of the mic
	public float GetHighestFreqOverall() {
		for (int i = 0; i < freqSubbandsInstant.Length; i++) {
			if (freqSubbandsInstant[i] > highestFreqOverall) highestFreqOverall = freqSubbandsInstant[i];
		}

		return highestFreqOverall;
	}
}
