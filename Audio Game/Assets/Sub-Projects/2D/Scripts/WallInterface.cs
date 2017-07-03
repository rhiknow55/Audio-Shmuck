using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WallInterface {

	void ColorLerp();

	void PosLerp();

	void SetInitialPos(Vector3 startingPos);

	void SetInitialColor(Color startingColor);

	void StartTimer();

	float TimePassed();
}
