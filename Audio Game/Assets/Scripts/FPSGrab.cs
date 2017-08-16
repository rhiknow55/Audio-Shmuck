using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGrab : MonoBehaviour {

	Camera parentCamera;
	RaycastHit hit;
	GameObject pickedUpObject;
	bool grabbing;

	void Start()
	{
		parentCamera = this.transform.parent.gameObject.GetComponent<Camera>();
	}

	void Update()
	{
		if (Input.GetKeyDown("e")) {
			if (grabbing) grabbing = false;
			else grabbing = true;
		}
		
		if (grabbing)
		{
			Ray ray = parentCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
			if (Physics.Raycast(ray, out hit))
			{
				if(hit.collider.gameObject.tag == "Cassette")
				{
					print("Cassette hit!");
					pickedUpObject = hit.collider.gameObject;
					pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
					hit.collider.gameObject.transform.parent = this.transform;
					hit.collider.gameObject.transform.localPosition = Vector3.zero;
				}
			}
		}else
		{
			if (pickedUpObject != null)
			{
				pickedUpObject.transform.parent = null;
				pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
			}
			pickedUpObject = null;
		}
	}
}
