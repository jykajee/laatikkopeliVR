using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour {

	// Use this for initialization
	void Start () {
		using (var ws = new WebSocket ("ws://dragonsnest.far/Laputa")) {
			ws.OnMessage += (sender, e) =>
				Debug.Log ("Laputa says: " + e.Data);

			ws.Connect ();
			ws.Send ("BALUS");
			Debug.Log (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
