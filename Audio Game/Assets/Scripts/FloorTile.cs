﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

	int freqBand;
	Renderer rend;
	float initialScaleY;
	float factor;

	void Start()
	{
		initialScaleY = transform.localScale.y;
	}

	void Update()
	{
		ScaleVertically();
	}

	public void Setup(int _freqBand, float _factor)
	{
		freqBand = _freqBand;
		factor = _factor;
		rend = gameObject.GetComponent<Renderer>();
	}

	void ScaleVertically()
	{
		float newScaleY = initialScaleY + AudioCompiler.instance.getFreqSubbandsInstant()[freqBand] * factor;

		transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);
	}

	public void SetColor(Color col)
	{
		rend.material.color = col;
	}
}
