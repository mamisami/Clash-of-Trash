using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TimerController : NetworkBehaviour {
	float timeLeft = 10.0f;

	Quaternion draggableRotation = Quaternion.Euler(-90, 0,0);

	// Use this for initialization
	void Start () {
		Random.InitState ((int)System.DateTime.Now.Ticks);
	}

	// Update is called once per frame
	void Update () {

		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			timeLeft = 10.0f;

			Object draggablePrefab = Resources.Load ("Prefabs/Draggable", typeof(GameObject));
			Vector3 position = new Vector3 (Random.Range(-25.0f, -10.0f), Random.Range(23.0f, 17.0f));
			GameObject draggableGameObject = Instantiate (draggablePrefab, position, draggableRotation) as GameObject;
			NetworkServer.Spawn (draggableGameObject);
		}
	}
}
