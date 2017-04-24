using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]


// Draggable object, when collising with another object, destroy
public class Draggable : MonoBehaviour {
	
	Trash trash = null;

	Vector3 screenPoint;
	Vector3 offset;

	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag(){
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
	}

	void OnMouseUp(){
		if (isObjectInTrash ()) {
			trash.Close ();
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider hit) {
		Trash hitTrash = hit.gameObject.GetComponent<Trash> ();

		if (hitTrash) {
			// Already a trash, close it
			if(trash)
				trash.Close ();
			trash = hitTrash;
			trash.Open ();
		}
	}

	void OnTriggerStay (Collider hit) {
		Trash hitTrash = hit.gameObject.GetComponent<Trash> ();

		// If no other trash, it's the current trash
		if (trash == null) {
			hitTrash.Open ();
			trash = hitTrash;
		}
	}

	void OnTriggerExit (Collider hit) {
		Trash trashExit = hit.gameObject.GetComponent<Trash> ();

		if (trashExit) {
			trashExit.Close ();

			if (trashExit == trash)
				trash = null;
		}
	}

	bool isObjectInTrash() {
		return trash != null;
	}
}

