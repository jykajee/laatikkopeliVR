using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBox : MonoBehaviour {

	private Queue q;

	// Use this for initialization
	void Start () {
		q = new Queue ();
	}
	
	// Update is called once per frame
	void Update () {
		updateText ();
	}

	public void Add(Vector3 v)
	{
		q.Enqueue(v);
	}

	private void updateText()
	{


		//this.GetComponent<UnityEngine.UI.Text>().text =  
	}
}
