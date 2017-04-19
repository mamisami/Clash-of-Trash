using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider))]

public class Draggable : MonoBehaviour {
	Vector3 screenPoint;
	Vector3 offset;
	// Use this for initialization
	void Start () {
		Debug.Log ("Test");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Awake(){

	
	}
	void OnMouseDown(){
		Debug.Log ("OnMouseDown");
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag(){
		Debug.Log ("OnMouseDrag");
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
	}
}

