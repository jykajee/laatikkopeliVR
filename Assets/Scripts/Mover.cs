using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public int treshold = 0;
	public int buffer = 5;
	public GameObject textBox;
	private Queue<int[]> values;


	// Use this for initialization
	void Start () {
		values = new Queue<int[]> (buffer);
	}
	
	// Update is called once per frame
	void Update () {
		//calculateDelta ();
	}

	public void NewValues(int[] v)
	{
		values.Enqueue (v);
		if (values.Count > 1)
			calculateDelta ();
			
	}

	void calculateDelta()
	{
		Vector3 dRotation = new Vector3 ();

		if (values.Count > 1) {	

			int[] v1 = values.Dequeue ();
			int[] v2 = values.Peek ();

			int dX = v1 [0] - v2 [0];
			int dY = v1 [0] - v2 [0];
			int dZ = v1 [0] - v2 [0];

			//Processing delta values
			/*
		if (dX > -treshold && dX < treshold) {
			dX = 0;
		}
		if (dY > -treshold && dY < treshold) {
			dY = 0;
		}
		if (dZ > -treshold && dZ < treshold) {
			dZ = 0;
		}
		*/

			dRotation.Set (-dX, -dY, -dZ);

			Debug.Log (dRotation);
			this.gameObject.transform.Rotate (dRotation);
		}
	}
}
