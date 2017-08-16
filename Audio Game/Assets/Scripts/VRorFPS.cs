using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRorFPS : MonoBehaviour {

	public static VRorFPS instance;
	
	public bool usingVR;
	public GameObject VRTK_SDKManagerGO;
	public GameObject VRTK_ScriptsGO;
	public GameObject FPSControllerGO;

	void Awake()
	{
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

		if (usingVR)
		{
			FPSControllerGO.SetActive(false);
			this.transform.GetChild(0).gameObject.SetActive(false);
		}
		else
		{
			VRTK_SDKManagerGO.SetActive(false);
			VRTK_ScriptsGO.SetActive(false);
		}
	}

	
}
