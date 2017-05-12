﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnManager : NetworkBehaviour {
	//float timeLeft = 1.0f;

	//int draggableID = 0;
	public GameObject[] draggables = new GameObject[Global.draggablesCoordinates.Length];

	Quaternion draggableRotation = Quaternion.Euler(-90, 0,0);

	// Use this for initialization
	void Start () {
		Random.InitState ((int)System.DateTime.Now.Ticks);

		for (int i = 0; i < Global.draggablesCoordinates.Length; i++)
			spawnDaggable (i);
	}

	// Update is called once per frame
	void Update () {

	}

	public void spawnDaggable(int draggableID) {
		Object draggablePrefab = Resources.Load ("Prefabs/Draggable", typeof(GameObject));
		GameObject draggableGameObject = Instantiate (draggablePrefab, Global.draggablesCoordinates[draggableID], draggableRotation) as GameObject;
		NetworkServer.Spawn (draggableGameObject);

		draggables [draggableID] = draggableGameObject;
		draggableGameObject.GetComponent<Draggable> ().realName = draggableID.ToString();
	}
}
