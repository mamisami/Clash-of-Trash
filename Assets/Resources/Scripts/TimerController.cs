using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimerController : NetworkBehaviour {
	float timeLeft = 1.0f;

	int draggableID = 0;
	public GameObject[] draggables = new GameObject[Global.MAX_DRAGGABLES];

	Quaternion draggableRotation = Quaternion.Euler(-90, 0,0);

	// Use this for initialization
	void Start () {
		Random.InitState ((int)System.DateTime.Now.Ticks);
	}

	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0) {
			timeLeft = 2.0f;

			Object draggablePrefab = Resources.Load ("Prefabs/Draggable", typeof(GameObject));
			Vector3 position = new Vector3 (Random.Range(-7.227f, 7.227f), Random.Range(23.0f, 17.0f));
			GameObject draggableGameObject = Instantiate (draggablePrefab, position, draggableRotation) as GameObject;
			NetworkServer.Spawn (draggableGameObject);

			int currentDraggableID = draggableID++;
			draggables [currentDraggableID] = draggableGameObject;
			draggableGameObject.GetComponent<Draggable> ().realName = currentDraggableID.ToString();
		}
	}
}
