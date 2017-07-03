using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	static int overallScore;
	public Text scoreText, multiplierText;
	public static float multiplier;

	void Start() {
		multiplier = 1;
	}

	void Update() {
		UpdateScore();
	}

	void UpdateScore() {
		scoreText.text = overallScore.ToString();
		multiplierText.text = "x "+ multiplier.ToString("f1");
	}

	public static void AddScore(int score) {
		overallScore += score;
	}
}
