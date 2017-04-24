using UnityEngine;
using System.Collections;

public class AudioScrub : MonoBehaviour {

	GameObject AudioFrom;//Used to hold the Audiosource
	GameObject AudioCompiler;
	private float scrollPos = 0f;//Position of Scroll
	bool Trig;// Used as a "gate"
	public string AudioObject; // This is the Game object which holds the audio source
	public string AudioCompilerObject;
	public AudioClip song;
	AudioSource compiler, player;

	float currentTime;
	bool paused;

	private void Start() {
		AudioFrom = GameObject.Find(AudioObject);//Get Audio from game object
		AudioCompiler = GameObject.Find(AudioCompilerObject);
		player = AudioFrom.GetComponent<AudioSource>();
		player.clip = song;
		//player.Pause();
		paused = true;
		compiler = AudioCompiler.GetComponent<AudioSource>();
		
		compiler.clip = song;
		compiler.Play();
	}

	void Update() {
		if (paused) CheckTimeAndUnpause();
	}

	void CheckTimeAndUnpause() {
		currentTime = Time.time;
		if (currentTime >= AudioPlayer.playDelay) {
			player.Play();
			paused = false;
		}
	}

	private void OnGUI() {

		scrollPos = GUI.HorizontalSlider(new Rect(0f, 50f, Screen.width, 50f), scrollPos, 0, AudioFrom.GetComponent<AudioSource>().clip.length);

		if (GUI.changed == true) {
			Trig = true;// Open "gate"
		}
		if (GUI.changed == false && !Input.GetMouseButton(0) && Trig == false)
        {
			scrollPos = AudioFrom.GetComponent<AudioSource>().time;// Makes slider follow the audio when not used (Clicked)
		}
		if (Input.GetMouseButtonUp(0) && Trig == true)
        {
			AudioFrom.GetComponent<AudioSource>().time = scrollPos;// Will only change the audio position once the mouse is released
			AudioCompiler.GetComponent<AudioSource>().time = scrollPos + 3f;
			Trig = false;
		}

		GUI.Label(new Rect(10f, 80f, 100f, 30f), (AudioFrom.GetComponent<AudioSource>().time).ToString());

	}
}