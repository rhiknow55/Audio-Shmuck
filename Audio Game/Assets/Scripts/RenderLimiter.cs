using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Limit's the player's vision to a certain units
/// </summary>


[RequireComponent(typeof(Camera))]
public class RenderLimiter : MonoBehaviour {

	Camera localCamera;

	void Start()
	{
		localCamera = this.GetComponent<Camera>();

		SetMaxRenderDistance(100f);
	}

	// Sets the max render distance, or the far clipping plane
	public void SetMaxRenderDistance(float _renderDistanceMax)
	{
		localCamera.farClipPlane = _renderDistanceMax;
	}
	
}
