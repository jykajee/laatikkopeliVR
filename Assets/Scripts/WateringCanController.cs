using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCanController : MonoBehaviour {
	private GameObject can;
	private GameObject waterParticles;
	// Use this for initialization
	void Start () {
		can = this.gameObject;
		waterParticles = this.gameObject.transform.GetChild (0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//Gizmos.DrawRay (transform.GetChild(0).transform.position, new Vector3(0, -500f,0), Color.red);
		Debug.DrawRay (transform.GetChild(1).transform.position, Vector3.down, Color.red);
		float angle = can.transform.rotation.eulerAngles.x;
		if (angle > 30f && angle < 90f) {
			WaterOn ();
			RaycastHit hit;
			Ray waterRay = new Ray (transform.GetChild(1).transform.position, Vector3.down);
			if (Physics.Raycast (waterRay, out hit)) {
				//Debug.Log("OSUMA REIKÄSTILLÄ ------------"+ hit.transform.gameObject.ToString());
				if (hit.transform.gameObject.tag == "Flower") {
					//Debug.Log ("KUKKARUUKKUUN OSUMA!!!!!---------");
					Watering (hit);
				}
			}
		} else {
			WaterOff ();
		}
	}

	private void WaterOn(){
		waterParticles.SetActive (true);
	}

	private void WaterOff(){
		waterParticles.SetActive (false);
	}

	private void Watering(RaycastHit hitFlower){
		hitFlower.transform.gameObject.GetComponentInParent<FlowerController> ().Grow ();
		//Debug.Log("Watering funktio kutsuttu!!!!!-----------------------");
	}
}
