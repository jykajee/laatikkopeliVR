using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour {

	public Text textBox;

	// 0 = leap motion 1 = Oculus controllers
	private int controllerSetting;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("c")) {
			ToggleController ();
		}
		if (Input.GetKeyDown ("s")) {
			StartLevel ();
		}
	}

	private void ToggleController(){
		if (controllerSetting == 0) {
			controllerSetting = 1;
			StartCoroutine (UpdateTextBox ("Controller setting: Oculus controllers"));
		} else if (controllerSetting == 1) {
			controllerSetting = 0;
			StartCoroutine(UpdateTextBox ("Controller setting: Leap Motion hand gestures"));
		}
	}

	IEnumerator UpdateTextBox(string msg){
		textBox.text = msg;
		yield return new WaitForSeconds(3);
		textBox.text = "";
	}

	private void StartLevel(){
		if (controllerSetting == 0) {
			SceneManager.LoadScene ("LeapSceneVR");
		} else if (controllerSetting == 1) {
			SceneManager.LoadScene ("OculusSceneVR");
		} else {
			Debug.Log ("LOL Can't load scene with this controllerSetting: " + controllerSetting);
		}
	}


}
