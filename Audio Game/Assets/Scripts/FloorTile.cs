using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

	int freqBand;
	Renderer rend;

	void Start()
	{
		
	}

	public void Setup(int _freqBad)
	{
		freqBand = _freqBad;
		rend = gameObject.GetComponent<Renderer>();
	}

	public void SetColor(Color col)
	{
		rend.material.color = col;
	}
}
