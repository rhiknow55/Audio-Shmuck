using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// PC: Any keyboard button can be pressed to the beat.
/// </summary>
public class InputHandler : MonoBehaviour 
{
	#region Variables
	public AtomicAttraction atomicAttraction;

	private float errorWindow;
	

	// For documenting FreqSpikes
	private int currentFreqSpikeIndex;
	private bool availableSpike;
	public List<float> compilerHistogram, playbackHistogram;
	#endregion

	#region Monobehaviour Methods
	private void Start()
	{
		errorWindow = AudioPlayback.instance.errorWindow;

		currentFreqSpikeIndex = 0;
		availableSpike = false;
		compilerHistogram = new List<float>();
		playbackHistogram = new List<float>();
	}
	private void Update()
	{
		if (!AudioPlayback.instance.AudioIsPlaying())
			return;

		if (playbackHistogram.Count > currentFreqSpikeIndex && compilerHistogram.Count > currentFreqSpikeIndex)
		{
			availableSpike = true;
		}
		else
		{
			availableSpike = false;
		}

		DocumentFreqSpikes();

		if(availableSpike) HandlePassingErrorWindow();

		if (Input.anyKeyDown)
		{
			HandleInput();
		}
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private/Protected Methods
	private void DocumentFreqSpikes()
	{
		// Document all the freqSpikes using a Vector2. 
		// Set the AudioPlayback's value to a crazy high value, so when determining pre-emptive input, if it qualifies AudioCompiler check, it will always pass AudioPlayback check (it hasn;t been overwritten with actual)

		if (AudioCompiler.instance.AudioIsPlaying())
		{
			if (AudioCompiler.instance.CheckForFreqSpike().Count > 0)
			{
				//print("Added: " + AudioCompiler.instance.GetLastFreqSpikeTime());
				float time = AudioCompiler.instance.GetLastFreqSpikeTime();
				compilerHistogram.Add(time);
				playbackHistogram.Add(time + errorWindow);
			}
		}
	}

	private void HandlePassingErrorWindow()
	{
		if (AudioPlayback.instance.AudioIsPlaying())
		{
			// If we have passed the current spike's time + errorWindow, we can no longer attempt that note.
			if (Time.time > playbackHistogram[currentFreqSpikeIndex] + errorWindow)
			{
				//print("Time.time: " + Time.time + "  ::  playback: " + playbackHistogram[currentFreqSpikeIndex] + " :: at index of: " + currentFreqSpikeIndex);
				UpdateComparisonIndex();
			}
		}
	}

	// Correct input, if within errorWindow of AudioCompiler's last FreqSpike and within AudioPlayback's last FreqSpike
	private void HandleInput()
	{
		if(!availableSpike)
		{
			atomicAttraction.IncorrectInputEffect();
			return;
		}

		if (Time.time - compilerHistogram[currentFreqSpikeIndex] >= 0.0f &&
			Time.time - playbackHistogram[currentFreqSpikeIndex] <= errorWindow)
		{
			// Correct input
			atomicAttraction.CorrectInputEffect();
			UpdateComparisonIndex();
		}
		else
		{
			atomicAttraction.IncorrectInputEffect();
		}
	}

	private void UpdateComparisonIndex()
	{
		currentFreqSpikeIndex++;
		//print("Updated");
	}
	#endregion
}
