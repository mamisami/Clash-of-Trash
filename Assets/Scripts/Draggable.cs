using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]


// Draggable object, when collising with another object, destroy
public class Draggable : MonoBehaviour {
	
	GameObject objectTrigerred = null;

	Vector3 screenPoint;
	Vector3 offset;

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

	void OnMouseUp(){
		if (isObjectInTrash ())
			Destroy (gameObject);
	}

	void OnTriggerEnter (Collider hit) {
		Debug.Log( "OnTriggerEnter" );
		objectTrigerred = hit.gameObject;
	}

	void OnTriggerExit (Collider hit) {
		Debug.Log( "OnTriggerExit" );
		objectTrigerred = null;
	}

	bool isObjectInTrash() {
		return objectTrigerred != null;
	}
}

