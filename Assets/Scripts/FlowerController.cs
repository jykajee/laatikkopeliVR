using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {
	private float speed = 0.02f;
	private bool grown = false;
	private bool growing = false;
	private Vector3 destination;
	private GameObject flower;
	private Vector3 growth = new Vector3 (0, 0.06f, 0);

	private GameController2 gameController2;
	// Use this for initialization
	void Start () {
		flower = transform.GetChild (0).gameObject;
		destination = flower.transform.position + growth;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (growing) {
			ContinueGrowing ();
		}
			
		
	}

	public void Grow(){
		if (grown == false) {
			grown = true;
			growing = true;
		}
	}

	public void Ungrow(){
		if (grown == true) {
			transform.position = transform.position - growth;
			grown = false;
		}
	}

	private void ContinueGrowing(){
		if (flower.transform.position != destination) {
			flower.transform.position = Vector3.Lerp (flower.transform.position, destination, speed + Time.deltaTime);
		} else {
			growing = false;
			grown = true;
			gameController2.NotifyGrowth (this);
		}
	}

	public void SetGameController2(GameController2 gc){
		gameController2 = gc;
	}
}
