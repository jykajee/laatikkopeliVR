using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameController : MonoBehaviour {

	public int objectsAmount;
	public Transform spawnPoint;
	public GameObject[] objects;
	public Text textBox;
	int objectsDestroyed;
	int objectsDropped;
	bool gameRunning;
	int[] roundTimes;

	// Use this for initialization
	void Start () {
		gameRunning = false;
		objectsDestroyed = 0;
		objectsDropped = 0;
		textBox.text = "Press (n) to start";
		roundTimes = new int[5] {1, 2, 3, 4, 5};
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("n") && !gameRunning)
		{
				StartGame();	
		}
	}

	public void ObjectDestroyed(GameObject obj)
	{
		Destroy (obj, 1);
		SpawnNew ();
		objectsDestroyed++;
		objectsAmount--;
		textBox.text = objectsDestroyed.ToString ();
	}

	public void OffTable (GameObject obj)
	{
		Destroy (obj, 1);
		SpawnNew ();
		objectsDropped++;
	}

	private void SpawnNew()
	{
		if (objectsAmount > 0) {
			GameObject obj = objects [Random.Range (0, objects.Length)];
			obj.GetComponent<ObjectController> ().SetGameController (this);
			Instantiate (obj, spawnPoint);
		} else
			EndGame ();
	}

	private void StartGame()
	{
		gameRunning = true;
		SpawnNew ();
		textBox.text = "0";
		WriteString ();
	}

	private void EndGame()
	{
		
	}

	private void WriteString()
	{
		string path = "Assets/Resources/test.txt";

		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);
		string[] lines = {
			"Test results: ",
			"Objects moved to target area: " + objectsDestroyed,
			"Objects dropped: " + objectsDropped,
			"Round times: " + string.Join(" ", new List<int>(roundTimes).ConvertAll(i => i.ToString()).ToArray())
		};
		foreach (string line in lines){
			writer.WriteLine(line);
		}
		writer.Close();

		//Re-import the file to update the reference in the editor
		AssetDatabase.ImportAsset(path); 
		TextAsset asset = (TextAsset) Resources.Load("test");

		//Print the text from the file
		Debug.Log(asset.text);
	}
		
}
