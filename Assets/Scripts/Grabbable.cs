using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour {

	public bool grabbed;


	private Rigidbody rb;
	private GameObject o;
	private GameObject mainCamera;
	private float distance;
	private Vector3 vel;

	// Use this for initialization
	void Start () {
		grabbed = false;
		rb = GetComponent<Rigidbody> ();
		o = GetComponent<GameObject> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (grabbed) 
		{
			Carry ();
		}

	}

	public void toggleGrab(GameObject carrier)
	{
		if (!grabbed) {
			grabbed = true;
			distance = Vector3.Distance (mainCamera.transform.position, rb.transform.position);
			Carry ();
		} else {
			grabbed = false;
			UnCarry (carrier);
		}
	}

	void Carry()
	{
		rb.isKinematic = true;
		//rb.useGravity = false;

		rb.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
	}
	void UnCarry(GameObject carrier)
	{
		rb.isKinematic = false;
		//rb.useGravity = true;
		vel = carrier.GetComponent<Rigidbody>().velocity;
		rb.AddForce(vel,ForceMode.VelocityChange);
	}

}
