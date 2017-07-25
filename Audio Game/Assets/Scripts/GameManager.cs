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
	public Transform[] rotatedFloorTransforms;

	public bool staticFloor;
	public bool rotatedFloor;
	public bool rotatingFloorVisualizer;

    void Start()
    {
        InitVisualizers();

    }

    void InitVisualizers()
    {
		if (staticFloor) InitStaticFloor();

		if (rotatedFloor) InitRotatedFloor();

	}

	void InitStaticFloor()
	{
		GameObject floorGO = Instantiate(GlobalManager.instance.GetFloorPrefab(), staticFloorTransform.position, Quaternion.identity);
		
		floorGO.AddComponent<FloorVisualizer>();
	}
	
	void InitRotatedFloor()
	{
		foreach(Transform rotatedFloorTransform in rotatedFloorTransforms)
		{
			GameObject floorGO = Instantiate(GlobalManager.instance.GetFloorPrefab(), rotatedFloorTransform.position, Quaternion.identity);

			floorGO.AddComponent<FloorVisualizer>();

			floorGO.GetComponent<FloorVisualizer>().Setup(rotatedFloorTransform.localRotation.eulerAngles, 90);
		}
	}

}
