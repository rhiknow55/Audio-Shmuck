using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

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

	void Awake()
	{
		if(VRorFPS.instance.usingVR) VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
	}

	void OnDestroy()
	{
		if(VRorFPS.instance.usingVR) VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
	}

	void Start()
    {
        InitVisualizers();
		AttachRenderLimiterToCamera();
    }

	void AttachRenderLimiterToCamera()
	{
		if (!Camera.main.gameObject.GetComponent<RenderLimiter>()) Camera.main.gameObject.AddComponent<RenderLimiter>();
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
