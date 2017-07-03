using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerScript : MonoBehaviour {

	enum DIRECTION { RIGHT, LEFT};
	public float xScaleModifier, yScaleModifier;
	bool rightState, leftState, centerState;

	float lerpTime = 0.1f;
	float currentLerpTime;

	Vector3 oldScale, newScale, oldPos, newPos;
	bool keyPressed, canPress;

	void Start() {
		oldScale = transform.localScale;
		newScale = transform.localScale;
		oldPos = transform.position;
		newPos = transform.position;
		currentLerpTime = -1f;
		centerState = true;
		canPress = false;
	}

	void Update() {
		if (canPress) GetKeyInput();

		if (!canPress) {
			currentLerpTime += Time.deltaTime;
			if (currentLerpTime >= lerpTime) {
				canPress = true;
				currentLerpTime = 0;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Wall") {

			transform.DOMove(new Vector3(newPos.x, newPos.y, transform.position.z - 10), lerpTime);
			
		}
	}

	void FixedUpdate() {
		
	}

	void GetKeyInput() {
		// CENTER STATE
		if (Input.GetKeyDown(KeyCode.RightArrow) && centerState && canPress) {
			centerState = false;
			rightState = true;
			leftState = false;
			canPress = false;
			DoScale(2, 40);
			DoMove(45);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) && centerState && canPress) {
			centerState = false;
			rightState = false;
			leftState = true;
			canPress = false;
			DoScale(2, 40);
			DoMove(-45);
		}

		// RIGHT STATE
		if (Input.GetKeyDown(KeyCode.LeftArrow) && rightState && canPress) {
			centerState = true;
			rightState = false;
			leftState = false;
			canPress = false;
			DoScale(10, 10);
			DoMove(-45);
		}

		// LEFT STATE
		if (Input.GetKeyDown(KeyCode.RightArrow) && leftState && canPress) {
			centerState = true;
			rightState = false;
			leftState = false;
			canPress = false;
			DoScale(10, 10);
			DoMove(45);
		}
	}

	void DoScale(float xScale, float yScale) {
		oldScale = transform.localScale;
		newScale = new Vector3(xScale, yScale, transform.localScale.z);
		transform.localScale = newScale;
		transform.DOPunchScale(newScale, lerpTime, 5, 0);
	}

	void DoMove(float xChange) {
		oldPos = transform.position;
		newPos = new Vector3(oldPos.x + xChange, transform.position.y + (newScale.y - oldScale.y) / 2, transform.position.z);
		transform.DOMove(newPos, lerpTime);
	}
}
