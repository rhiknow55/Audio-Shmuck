using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Ryan Oh
/// 
/// 
/// </summary>
public class GameManager : MonoBehaviour {

	public Transform staticFloorTransform;
	public Transform dynamicTransformsParent;

	public bool staticFloor;
	public bool rotatedFloor;
	public bool rotatingFloorVisualizer;

    void Start()
    {
        InitVisualizers();
		AttachRenderLimiterToCamera();
    }

	void AttachRenderLimiterToCamera()
	{
		if (!GlobalManager.instance.GetEyeCameraGO().GetComponent<RenderLimiter>()) GlobalManager.instance.GetEyeCameraGO().AddComponent<RenderLimiter>();
	}




    void InitVisualizers()
    {
		//if (staticFloor) InitStaticFloor();

		if (rotatedFloor) InitRotatedFloor();

	}

	void InitStaticFloor()
	{
		GameObject floorGO = Instantiate(GlobalManager.instance.GetFloorPrefab(), staticFloorTransform.position, Quaternion.identity);
		
		floorGO.AddComponent<FloorVisualizer>();
	}
	
	void InitRotatedFloor()
	{
		foreach(Transform containerTransform in dynamicTransformsParent)
		{
			foreach (Transform childTransform in containerTransform)
			{
				GameObject floorGO = Instantiate(GlobalManager.instance.GetFloorPrefab(), childTransform.position, Quaternion.identity);

				floorGO.AddComponent<FloorVisualizer>();

				floorGO.GetComponent<FloorVisualizer>().Setup(childTransform.localRotation.eulerAngles, childTransform.eulerAngles.z);
			}
		}
	}

}
