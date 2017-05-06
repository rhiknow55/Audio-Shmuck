using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Steps:
/// #1: Use Fast Fourier Transform to convert audio into a list of 1024 frequencies.
///    In total, a song with sample frequency of 44100 would have 43 of these 1024 lists occur in one second.
/// #2: Take the list of samples from both left and right to create a sterea sample list
/// #3: Create numberOfSubbands amount of subbands to separate the 1024 frequencies into.
///     My current algorithm is: Summation from (lower limit of n=1) to (upper limit of N=freqSubbandsInstant.Length)
///                              of (1024/K) * log(n)/log(K)
///     Where variable K is (13/16) * N
///     If summation of all the values is greater than 1024, it will print an error, and not create a proper list.
/// #4: Add one second's worth of subband instant freq into freqSubbandsAverageLocal at the correct index.
/// #5: Check if the freqSubbandsInstant[n] is greater than the average of freqSubbandsAverageLocal[n]
///     So basically if the instant freq spikes
/// #6: Calculate the variance of each subband

[RequireComponent (typeof (AudioSource))]
public class AudioCompiler : MonoBehaviour {
    AudioSource audioSource;

	// Private Lists
    private float[] samplesLeft = new float[1024];
	private float[] samplesRight = new float[1024];
	private float[] samplesStereo = new float[1024];
	public static float[] freqSubbandsInstant;
	private float[][] freqSubbandsAverageLocal;
	private float[] variances;
	private float[] constantC;

	// Public Variables
	public int numberOfSamples;
	public int numberOfSubbands;
	float varianceLimit = 0.25f;

	//Private Variables
	int averageLocalIndex;
	int count = 1;

	void Start () {
        audioSource = GetComponent<AudioSource>();
		freqSubbandsInstant = new float[numberOfSubbands];
		// Instantiate a multidimensional array like this - each subband has 43 values in one second
		freqSubbandsAverageLocal = new float[numberOfSubbands][];
		for (int i = 0; i < numberOfSubbands; i++) {
			freqSubbandsAverageLocal[i] = new float[43];
		}
		// Instantiate the list of variances of the subbands
		variances = new float[numberOfSubbands];
		constantC = new float[numberOfSubbands];
	}
	
	void Update () {
        GetSpectrumAudioSource();
		CreateStereoSampleList();
		CreateSubbands();
		CalculateVarianceOfSubbands();
		List<int> testList = CheckForFreqSpike();
		//if(testList.Count != 0) Debug.Log(testList.Count);
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
		for(int i = 1; i <= numberOfSubbands; i++) {
			int numberOfSamplesToAddToSubband = (int) (Mathf.Log(i, logBaseToGetClosestTo1024) * (numberOfSamples / numberOfSubbands)) + 1; // Reason I add 1, is b/c otherwise log(1) is null and causes errors
			float average = 0;
			// On the last subband, add the rest of sample frequencies
			if(i == numberOfSubbands) {
				int leftover = numberOfSubbands - count;
				numberOfSamplesToAddToSubband = leftover;
			}

			for(int j = 0; j < numberOfSamplesToAddToSubband; j++) {
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
		for(int i = 0; i < freqSubbandsInstant.Length; i++) {
			freqSubbandsAverageLocal[i][averageLocalIndex] = freqSubbandsInstant[i];
		}
		if (averageLocalIndex < 43 - 1) averageLocalIndex++;
		else averageLocalIndex = 0;
	}

	// Step #5
	public List<int> CheckForFreqSpike() {
		List<int> spikeList = new List<int>();
		for(int i = 0; i < numberOfSubbands; i++) {
			float localAverage = 0;
			for (int j = 0; j < 43; j++) {
				localAverage += freqSubbandsAverageLocal[i][j];
			}
			localAverage /= 43;
			
			if(freqSubbandsInstant[i] > localAverage * constantC[i] && variances[i] > varianceLimit) {
				//Debug.Log("localAverage = " + localAverage * C + " ||||| instant = " + freqSubbandsInstant[i]);
				spikeList.Add(i);
			}
		}

		return spikeList;
	}

	// Step #6
	void CalculateVarianceOfSubbands() {
		for(int i = 0; i < variances.Length; i++) {
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
}
