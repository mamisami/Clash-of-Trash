using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void moveDraggable(string name, Vector3 position) {
		if (!isLocalPlayer)
			return;

		CmdMoveDraggable (name, position);
	}

	[Command]
	void CmdMoveDraggable(string name, Vector3 position) {
		GameObject[] draggables = GameObject.FindGameObjectsWithTag("Draggable");
		foreach (GameObject draggableObject in draggables) {
			Draggable draggable = draggableObject.GetComponent<Draggable>();
			if (draggable.realName == name) {
				draggable.transform.position = position;
				return;
			}
		}
	}
}
