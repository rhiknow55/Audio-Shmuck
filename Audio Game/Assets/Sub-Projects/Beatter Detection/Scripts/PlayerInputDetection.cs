using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputDetection : MonoBehaviour {

	// Public Variables
	public static List<GameObject> triggeredIndicators;
	public GameObject backdrop;

	// Private Varibles
	Color backdropOriginalColor;

	void Start() {
		triggeredIndicators = new List<GameObject>();
		backdropOriginalColor = backdrop.GetComponent<MeshRenderer>().material.color;
	}

	void Update() {
		KeyDetection();
	}

	// Backdrop color changing
	void FlashBackdrop(Color c) {
		backdrop.GetComponent<MeshRenderer>().material.color = c;
		StartCoroutine(FadeBack());
	}

	IEnumerator FadeBack() {
		float elapsedTime = 0f;
		float totalTime = 0.2f;
		Color currentColor = backdrop.GetComponent<MeshRenderer>().material.color;
		while (elapsedTime < totalTime) {
			elapsedTime += Time.deltaTime;
			backdrop.GetComponent<MeshRenderer>().material.color = Color.Lerp(currentColor, backdropOriginalColor, elapsedTime / totalTime);
			yield return null;
		}
	}



	//----------------------------------------------------------------
	// Key detection

	void CorrectKeyPress() {
		// TODO: Background flashes green
		FlashBackdrop(Color.green);
		if (triggeredIndicators.Count > 0) triggeredIndicators[0].GetComponent<IndicatorScript>().BackToPool();
	}

	void WrongKeyPress() {
		//TODO: Background flashes red
		FlashBackdrop(Color.red);
		if (triggeredIndicators.Count > 0) triggeredIndicators[0].GetComponent<IndicatorScript>().BackToPool();
	}

	void KeyPressedWithNoIndicator() {
		if (triggeredIndicators.Count == 0) {
			WrongKeyPress();
			throw new System.Exception();
		}
	}

	void KeyDetection() {
		if (Input.GetKeyDown(KeyCode.Keypad1)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if(triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 1) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 1");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 1");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad2)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 2) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 2");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 2");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad3)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 3) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 3");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 3");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad4)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 4) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 4");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 4");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad5)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 5) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 5");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 5");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad6)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 6) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 6");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 6");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad7)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 7) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 7");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 7");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad8)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 8) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 8");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 8");
				WrongKeyPress();
			}
		}
		if (Input.GetKeyDown(KeyCode.Keypad9)) {
			try {
				KeyPressedWithNoIndicator();
			} catch {
				return;
			}
			if (triggeredIndicators[0].GetComponent<IndicatorScript>().GetRandomNumber() == 9) {
				// TODO: Player moves to corresponding block to dodge incoming blocks
				print("Correct 9");
				CorrectKeyPress();
			} else {
				// TODO: Destroys current indicator and player gets crushed
				print("Wrong 9");
				WrongKeyPress();
			}
		}
	}
}
