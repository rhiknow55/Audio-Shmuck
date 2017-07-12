using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// @author Ryan Oh
/// 
/// 
/// </summary>
public class GlobalManager : MonoBehaviour {

	public static GlobalManager instance;

	// -----VRTK GameObjects-----
	public GameObject VRTKGO;
	GameObject CameraRigGO;
	GameObject LeftControllerWithScriptsGO, RightControllerWithScriptsGO;
	GameObject ModelControllerLeftGO, ModelControllerRightGO;
	GameObject PlayAreaGO;
	GameObject EyeCameraGO;

    // -----ENVIRONMENT GameObjects-----
    public GameObject FloorGO;
    public GameObject TileGO;


    AudioClip selectedSong;

	void Awake()
	{
		// The Start method is called once VRTK_SDKManager is initialized
		VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);

		// Check if instance already exists
		if (instance == null)
			// if not, set instance to this
			instance = this;
		// If instance already exists (not null) and it is not this
		else if (instance != this)
			// Then destroy this gameobject. This enforces our singleton pattern, meaning there can only ever be one instance of this manager
			Destroy(this.gameObject);

		// Sets this to not be destroyed when reloading or changing scenes.
		DontDestroyOnLoad(this.gameObject);
	}

	void OnDestroy()
	{
		VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
	}

	void OnStart()
	{
		InitVRTKGameObjects();
	}

	/// <summary>
	/// Set the selected song to use for everything in this playthrough.
	/// </summary>
	/// <param name="song"></param>
	public void SetSelectedSong(AudioClip song)
	{
        print("Set selected song to " + song.name); 
		selectedSong = song;
	}

	/// <summary>
	/// Returns the current selected song.
	/// </summary>
	/// <returns></returns>
	public AudioClip GetSelectedSong()
	{
		return selectedSong;
	}


	void InitVRTKGameObjects()
	{
		CameraRigGO = VRTKGO.transform.Find("SDKSetups").Find("SteamVR").Find("[CameraRig]").gameObject;

		LeftControllerWithScriptsGO = VRTKGO.transform.Find("LeftController").gameObject;
		RightControllerWithScriptsGO = VRTKGO.transform.Find("RightController").gameObject;

		ModelControllerLeftGO = CameraRigGO.transform.Find("Controller (left)").gameObject;
		ModelControllerRightGO = CameraRigGO.transform.Find("Controller (right)").gameObject;

		PlayAreaGO = VRTKGO.transform.Find("PlayArea").gameObject;

		EyeCameraGO = CameraRigGO.transform.Find("Camera (eye)").gameObject;
	}

	// -----GET METHODS-----

	/// <summary>
	/// Returns the VRTK GameObject.
	/// </summary>
	public GameObject GetVRTKGO() { return VRTKGO; }

	/// <summary>
	/// Returns the CameraRig GameObject.
	/// </summary>
	public GameObject GetCameraRigGO() { return CameraRigGO; }

	/// <summary>
	/// Returns the LeftController GameObject (The GO with scripts)
	/// </summary>
	public GameObject GetLeftControllerWithScriptsGO() { return LeftControllerWithScriptsGO; }

	/// <summary>
	/// Returns the RightController GameObject (The GO with scripts)
	/// </summary>
	public GameObject GetRightControllerWithScriptsGO() { return RightControllerWithScriptsGO; }

	/// <summary>
	/// Returns the Controller (left) GameObject. (The GO with the model)
	/// </summary>
	public GameObject GetModelControllerLeftGO() { return ModelControllerLeftGO; }

	/// <summary>
	/// Returns the Controller (right) GameObject. (The GO with the model)
	/// </summary>
	public GameObject GetModelControllerRightGO() { return ModelControllerRightGO; }

	/// <summary>
	/// Returns the PlayArea GameObject.
	/// </summary>
	public GameObject GetPlayAreaGO() { return PlayAreaGO; }

	/// <summary>
	/// Retsurns the actual GO that holds the camera.
	/// </summary>
	/// <returns></returns>
	public GameObject GetEyeCameraGO() { return EyeCameraGO; }



    public GameObject GetFloorGO() { return FloorGO; }

    public GameObject GetTileGO() { return TileGO; }
}
