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
		foreach(Transform childTransform in dynamicTransformsParent)
		{
			GameObject floorGO = Instantiate(GlobalManager.instance.GetFloorPrefab(), childTransform.position, Quaternion.identity);

			floorGO.AddComponent<FloorVisualizer>();

			floorGO.GetComponent<FloorVisualizer>().Setup(childTransform.localRotation.eulerAngles, 45);
		}
	}

}
