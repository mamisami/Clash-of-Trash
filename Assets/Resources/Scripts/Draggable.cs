using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]


// Draggable object, when collising with another object, destroy
public class Draggable : NetworkBehaviour {
	
	Trash trash = null;

	Vector3 screenPoint;
	Vector3 offset;

	[Command]
	public void CmdAssignObjectAuthorityToClient(GameObject go)
	{
		go.GetComponentInChildren<NetworkIdentity>().AssignClientAuthority(this.GetComponentInChildren<NetworkIdentity>().connectionToClient);
	}

	[Command]
	public void CmdRemoveObjectAuthorityToClient(GameObject go)
	{
		go.GetComponentInChildren<NetworkIdentity>().RemoveClientAuthority(this.GetComponentInChildren<NetworkIdentity>().connectionToClient);
	}

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

