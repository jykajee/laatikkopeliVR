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
	public Text systemTextBox;
	public TextMesh userTextBox;
	public GameObject targetCenterPoint;
	public GameObject wall;
	public GameObject targetPad;
	public GameObject wateringCan;
	public GameObject[] flowers;
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
	int flowerGameLevel;
	String currentStatusMessage;
	Vector3 wateringStartPos;

	// Use this for initialization
	void Start () {
		gameRunning = false;
		objectsDestroyed = 0;
		totalObjectsDropped = 0;
		objectsDone = 0;
		systemTextBox.text = "Press (n) to start";
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
		userTextBox.text = "Aloitetaan";
		wateringStartPos = wateringCan.transform.position;
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
		if (Input.GetKeyDown ("f")) {
			StartFlowerGame ();
		}
		if (Input.GetKeyDown ("r")) {
			ResetWateringCan ();
		}
		if (Input.GetKeyDown ("l")) {
			LoadNextScene ();
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
		systemTextBox.text = objectsDestroyed.ToString ();
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
			systemTextBox.text = "Starting next round...";
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
		systemTextBox.text = "Sort the correct cubes into the box";
	}

	private void StartGame()
	{
		wall.SetActive (true);
		targetPad.SetActive (true);
		ClearFlowerGame ();
		gameRunning = true;
		SetupTable ();
		systemTextBox.text = "Sort the correct cubes into the box";
		UpdateUIMessage ("Lajittele vihreät kuutiot kohteeseen");
		currentRound = 1;
	}

	private void EndGame()
	{
		systemTextBox.text = "This part is done. Remove VR headset.";
		UpdateUIMessage("Lajittelutehtävä ohi. Voit ottaa lasit pois.");
		WriteString ();
		ClearTable ();
	}
	/*
	IEnumerator WaitSome(){
		print ("before waiting some");
		yield return new WaitForSeconds (2);
		print ("after waiting some");
	}*/

	private void ClearTable(){
		if (instantiatedObjects != null) {
			while (instantiatedObjects.Count != 0) {
				Destroy (instantiatedObjects.ElementAt (0));
				instantiatedObjects.RemoveAt (0);
			}
		}
	}

	private void StartFlowerGame(){
		UpdateUIMessage ("Kastele kukat");
		ClearTable ();
		wall.SetActive (false);
		targetPad.SetActive (false);
		wateringCan.gameObject.SetActive (true);
		flowerGameLevel = 0;
		foreach (GameObject flower in flowers){
			flower.SetActive (false);
			flower.GetComponent<FlowerController>().Ungrow ();
			flower.GetComponent<FlowerController> ().SetGameController2 (this);
		}
		NextFlowerGameLevel ();

	}
	public void NextFlowerGameLevel(){
		if (flowerGameLevel == 0) {
			flowers [0].SetActive (true);
			flowerGameLevel++;
		} else {
			flowers [flowerGameLevel-1].SetActive (false);

			if (flowers.Length > flowerGameLevel) {
				flowers [flowerGameLevel].SetActive (true);
				flowerGameLevel++;
			} else {
				UpdateUIMessage ("Kaikki kukat kasteltu.");
			}
		}
		 
	}

	public void UpdateUIMessage (String msg) {
			userTextBox.text = msg;
	}

	public void NotifyGrowth(FlowerController fc ){
		NextFlowerGameLevel ();
	}

	private void ResetWateringCan(){
		wateringCan.transform.rotation = new Quaternion (0, 0, 0, 0);
		wateringCan.transform.position = wateringStartPos;
	}

	private void ClearFlowerGame(){
		wateringCan.SetActive (false);
		foreach (GameObject flower in flowers){
			flower.SetActive (false);
		}
	}

	private void LoadNextScene(){
		if (SceneManager.GetActiveScene ().name == "LeapSceneVR") {
			SceneManager.LoadScene ("Start");
		}
		if (SceneManager.GetActiveScene ().name == "OculusSceneVR") {
			SceneManager.LoadScene ("StartLeap");
		}
	}

	private void WriteString()
	{
		int introRows = 7;
		int rowsPerRound = 3;
		string path = "Assets/Resources/test.txt";
		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path , true);
		string[] allData = new string[rounds * rowsPerRound + introRows];
		//Adding the test info into the allData array
		allData [0] = "-----";
		allData [1] = System.DateTime.Now.ToString ("dd_MM_yyyy_HH:mm");
		allData [2] =  SceneManager.GetActiveScene().name;
		allData [3] = "Total objects to target," + objectsDestroyed + "," + "Total dropped," + totalObjectsDropped;
		allData [4] = "Round times," + string.Join (",", new List<float> (roundTimes).ConvertAll (i => i.ToString ()).ToArray ());
		allData [5] = "Objects done each round," + string.Join (" ", new List<int> (objectsDoneEachRound).ConvertAll (i => i.ToString ()).ToArray ());
		allData [6] = "Objects dropped each round," + string.Join (" ", new List<int> (objectsDroppedEachRound).ConvertAll (i => i.ToString ()).ToArray());

		//Going through every round
		int currentRoundCounter = 0;
		for (int i = introRows-1; currentRoundCounter < rounds; i = (currentRoundCounter * rowsPerRound) + introRows-1 ){

			string distancesRow = "";
			for(int j = 0; j < targetObjectAmount; j++){
				distancesRow = distancesRow + "," + distancesFromTarget [currentRoundCounter] [j].ToString();
			}

			allData[i+1] = "Round,"+(currentRoundCounter+1);
			allData[i+2] = "Time,"+ roundTimes[currentRoundCounter].ToString();
			allData[i+3] = "Distances"+ distancesRow;

			currentRoundCounter++;
		}

		//Writing all data into file with writer
		for(int i = 0; i < (rounds * rowsPerRound) + introRows; i++){
			Debug.Log ("Writing line: " + allData [i]);
			writer.WriteLine(allData[i]);
		}
		writer.Close();

		//Re-import the file to update the reference in the editor
		AssetDatabase.ImportAsset(path); 
		TextAsset asset = (TextAsset) Resources.Load("test");

		//Print the text from the file
		Debug.Log(asset.text);
	}

}
