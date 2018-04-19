using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class GameController2 : MonoBehaviour {

	public int targetObjectAmount;
	public int rounds;
	public Transform[] spawnPoints;
	public GameObject targetObject;
	public GameObject[] otherObjects;
	public Text textBox;
	public GameObject targetCenterPoint;
	int objectsDestroyed;
	int totalObjectsDone;
	int totalObjectsDropped;
	int objectsDone;
	int objectsDropped;
	int[] objectsDoneEachRound;
	int[] objectsDroppedEachRound;
	bool gameRunning;
	float timer;
	float[] roundTimes;
	int currentRound;
	List<GameObject> instantiatedObjects;
	int escPresses;
	Vector3 targetPos;
	float[][] distancesFromTarget;
	int objectIndex;


	// Use this for initialization
	void Start () {
		gameRunning = false;
		objectsDestroyed = 0;
		totalObjectsDropped = 0;
		objectsDone = 0;
		textBox.text = "Press (n) to start";
		roundTimes = new float[rounds];
		escPresses = 0;
		objectsDoneEachRound = new int[rounds];
		objectsDroppedEachRound = new int[rounds];
		targetPos = targetCenterPoint.transform.position;
		distancesFromTarget = new float[rounds][];
		for (int x = 0; x < rounds; x++)
		{
			distancesFromTarget[x] = new float[targetObjectAmount];
		}
		objectIndex = 0;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (Input.GetKeyDown("n") && !gameRunning)
		{
			StartGame();	
		}
		if (Input.GetKeyDown ("p")) {
			NextRound ();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			escPresses++;
			if (escPresses > 1) {
				SceneManager.LoadScene ("Start");
			}
		}
	}

	public void ObjectHitTarget(GameObject obj)
	{
		float distanceFromTarget = Mathf.Round(((targetPos - obj.transform.position).magnitude)*1000f);
		distancesFromTarget [currentRound] [objectIndex] = distanceFromTarget;
		objectIndex++;
		Destroy (obj, 1);
		//SpawnNew ();
		objectsDestroyed++;
		targetObjectAmount--;
		textBox.text = objectsDestroyed.ToString ();
		TargetObjectDone ();
	}

	public void OffTable (GameObject obj)
	{
		if (obj.gameObject.tag == targetObject.gameObject.tag) {
			TargetObjectDone ();
		}

		Destroy (obj, 1);
		//SpawnNew ();
		objectsDropped++;

	}

	/*
	private void SpawnNew()
	{
		if (objectsAmount > 0) {
			GameObject obj = objects [Random.Range (0, objects.Length)];
			obj.GetComponent<ObjectController> ().SetGameController (this);
			Instantiate (obj, spawnPoint);
		} else
			EndGame ();
	}
*/
	private void TargetObjectDone(){
		objectsDone++;
		if (objectsDone >= targetObjectAmount) {
			NextRound ();
		}
	}

	private void NextRound(){
		roundTimes [currentRound - 1] = timer;
		objectsDoneEachRound [currentRound - 1] = objectsDone;
		objectsDroppedEachRound [currentRound - 1] = objectsDropped;
		currentRound++;
		totalObjectsDropped = +objectsDropped;
		objectsDone = 0;
		objectsDropped = 0;
		objectIndex = 0;
		if (currentRound > rounds) {
			EndGame ();			
		} else {
			ClearTable ();
			textBox.text = "Starting next round...";
			//StartCoroutine (WaitSome());
			SetupTable ();
		}
	}

	private void SetupTable(){
		int points = spawnPoints.Length;
		List<GameObject> roundObjects = new List<GameObject> ();
		instantiatedObjects = new List<GameObject>();

		//randomizing target positions
		//fill all targets
		for (int i = 0; i< targetObjectAmount; i++){
			roundObjects.Add (targetObject);
		}
		//putting filler objects into random spots
		int fillerObjectAmount = points - targetObjectAmount;
		int fillerCounter = 0;
		int spot;

		for (int i = 0; i < fillerObjectAmount; i++) {
			spot = UnityEngine.Random.Range(0, roundObjects.Count()+1);
			if (fillerCounter >= otherObjects.Count()) {
				fillerCounter = 0;
			}
			roundObjects.Insert (spot, otherObjects [fillerCounter]);
			fillerCounter++;
		}
		//filling spawn points
		for(int i = 0; i<spawnPoints.Length; i++){
			GameObject obj = roundObjects.ElementAt (i);

			obj.GetComponent<ObjectController> ().SetGameController2 (this);
			instantiatedObjects.Add( Instantiate (obj, spawnPoints [i]));
		}
		timer = 0;
		textBox.text = "Sort the correct cubes into the box";
	}

	private void StartGame()
	{
		gameRunning = true;
		SetupTable ();
		textBox.text = "Sort the correct cubes into the box";
		currentRound = 1;
	}

	private void EndGame()
	{
		textBox.text = "This part is done. Remove VR headset.";
		WriteString ();
	}
	/*
	IEnumerator WaitSome(){
		print ("before waiting some");
		yield return new WaitForSeconds (2);
		print ("after waiting some");
	}*/

	private void ClearTable(){

		while (instantiatedObjects.Count != 0) {
			Destroy (instantiatedObjects.ElementAt (0));
			instantiatedObjects.RemoveAt (0);
		}
	}

	private void WriteString()
	{
		string path = "Assets/Resources/test.txt";
		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);
		string[][] allData = new string[rounds + 1][];
		//Adding the test info into the allData array
		allData[0] = new string[] {
			"-----",
			System.DateTime.Now.ToString("dd_MM_yyyy_HH:mm"),
			SceneManager.GetActiveScene().name,
			"Total objects to target," + objectsDestroyed,
			"Total dropped," + totalObjectsDropped,
			"Round times," + string.Join(" ", new List<float>(roundTimes).ConvertAll(i => i.ToString()).ToArray()),
			"Objects done each round," + string.Join (" ", new List<int> (objectsDoneEachRound).ConvertAll (i => i.ToString ()).ToArray ()),
			"Objects dropped each round," + string.Join (" ", new List<int> (objectsDroppedEachRound).ConvertAll (i => i.ToString ()).ToArray ())
		};
		//Going through every round
		for(int i = 1; i <= rounds+1; i++){
			//Creating the row for distances from target
			string distancesRow = "Distances";
			for(int j = 0; j < targetObjectAmount; j++){
				distancesRow = distancesRow + "," + distancesFromTarget [i] [j].ToString();
			}
			//Adding round data to allData
			allData [i] = new string[] {
				"Round," + i,
				"Time," + roundTimes [i - 1].ToString (),
				distancesRow
			};
		};

		//Writing all data into file with writer
		for(int i = 0; i <= rounds+1; i++){
			foreach (string line in allData[i]){
				writer.WriteLine(line);
			}
		}
		writer.Close();

		//Re-import the file to update the reference in the editor
		AssetDatabase.ImportAsset(path); 
		TextAsset asset = (TextAsset) Resources.Load("test");

		//Print the text from the file
		Debug.Log(asset.text);
	}

}
