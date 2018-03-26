﻿using UnityEngine;
using System.IO;
using System;
using NAudio;
using NAudio.Wave;

public static class NAudioPlayer
{
	public static AudioClip FromMp3Data(string filePath)
	{
		// Load the data into a stream
		//MemoryStream mp3stream = new MemoryStream(data);
		// Convert the data in the stream to WAV format
		//Debug.Log(mp3stream);
		Mp3FileReader mp3audio = new Mp3FileReader(filePath);
		WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(mp3audio);
		// Convert to WAV data
		WAV wav = new WAV(AudioMemStream(waveStream).ToArray());
		
		AudioClip audioClip = AudioClip.Create(filePath, wav.SampleCount, 1, wav.Frequency, false);
		audioClip.SetData(wav.LeftChannel, 0);
		// Return the clip
		return audioClip;
	}

	private static MemoryStream AudioMemStream(WaveStream waveStream)
	{
		MemoryStream outputStream = new MemoryStream();
		using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, waveStream.WaveFormat))
		{
			byte[] bytes = new byte[waveStream.Length];
			waveStream.Position = 0;
			waveStream.Read(bytes, 0, Convert.ToInt32(waveStream.Length));
			waveFileWriter.Write(bytes, 0, bytes.Length);
			waveFileWriter.Flush();
		}
		return outputStream;
	}
}
