using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamMouseLook : MonoBehaviour {

	Vector2 mouseLook;
	Vector2 smoothV;
	public float sensitivity = 5.0f;
	public float smoothing = 2.0f;
	public Image normalCursor;
	public Image grabCursor;

	Ray ray;
	RaycastHit hit;

	GameObject character;

	// Use this for initialization
	void Start () {
		character = this.transform.parent.gameObject;
		grabCursor.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		smoothV.x = Mathf.Lerp (smoothV.x, md.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, md.y, 1f / smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp (mouseLook.y, -90f, 90f);

		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up);

		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider.CompareTag("Grabable"))
				{
					normalCursor.enabled = false;
					grabCursor.enabled = true;
					if (Input.GetMouseButtonDown (0)) 
					{
					hit.collider.gameObject.GetComponent<Grabbable> ().toggleGrab (character);
					}
				}
				else
				{
					normalCursor.enabled = true;
					grabCursor.enabled = false;
				}
			}
	}


}
