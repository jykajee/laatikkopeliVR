using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectController : MonoBehaviour {

	public GameController gameController;
	public GameController2 gameController2;
	bool beingDestroyed;


	// Use this for initialization
	void Start () {
		beingDestroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		//taking both gameControllers into consideration (BAD BUBBLEGUM)
		if (gameController != null) {
			if (other.gameObject.CompareTag (this.gameObject.tag) && !beingDestroyed) {
				gameController.ObjectDestroyed (this.gameObject);
				beingDestroyed = true;
			} else if (other.gameObject.CompareTag ("OffTable") && !beingDestroyed) {
				gameController.OffTable (this.gameObject);
			}
		} else {
			if (other.gameObject.CompareTag (this.gameObject.tag) && !beingDestroyed) {
				gameController2.ObjectHitTarget (this.gameObject);
				beingDestroyed = true;
			} else if (other.gameObject.CompareTag ("OffTable") && !beingDestroyed) {
				gameController2.OffTable (this.gameObject);
			}
		}
	}

	public void SetGameController(GameController gc)
	{
		gameController = gc;
	}
	public void SetGameController2(GameController2 gc2)
	{
		gameController2 = gc2;
	}
}
