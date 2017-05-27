using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnManager : NetworkBehaviour {
	float time = 0.0f;
	float timeOffset = 1.0f;

	//int draggableID = 0;
	public GameObject[] draggables;
	public double[] draggablesSpawnTime;

	Quaternion draggableRotation = Quaternion.Euler(0, 0,0);

	private GameObject[] draggablesPrefabs;

	// Use this for initialization
	void Start () {
		int nbDragabbles = Global.draggablesCoordinates.Length;// - (Global.level==2 ? 6 : 0);

		draggables = new GameObject[Global.draggablesCoordinates.Length];
		draggablesSpawnTime = new double[nbDragabbles];

		draggablesPrefabs = Resources.LoadAll<GameObject>(Global.WASTES_PATH);

		Random.InitState ((int)System.DateTime.Now.Ticks);

		for (int i = 0; i < nbDragabbles; i++)
			makeSpawn (i);
	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if (time > timeOffset) {
			timeOffset++;

			for (int i = 0; i < draggablesSpawnTime.Length; i++) {
				if (draggablesSpawnTime[i] != -1.0) {
					if (draggablesSpawnTime[i] < time && !Global.isFinish)
						makeSpawn(i);
				}
			}
		}
	}

	public void spawnDraggable(int draggableID) {
		draggablesSpawnTime[draggableID] = time + Random.Range (Global.SPAWN_TIME_MIN, Global.SPAWN_TIME_MAX+1);
	}

	private void makeSpawn(int draggableID) {
		draggablesSpawnTime[draggableID] = -1.0;

		//Object draggablePrefab = Resources.Load ("Prefabs/Draggable", typeof(GameObject));
		GameObject draggablePrefab = draggablesPrefabs [Random.Range (0, draggablesPrefabs.Length)];
		GameObject draggableGameObject = Instantiate (draggablePrefab, Global.draggablesCoordinates[draggableID], draggableRotation) as GameObject;
		NetworkServer.Spawn (draggableGameObject);

		draggables [draggableID] = draggableGameObject;
		draggableGameObject.GetComponent<Draggable> ().realName = draggableID.ToString();
	}

	public void HideAll(){
		for (int i = 0; i < draggables.Length; i++)
			if (draggables [i]) {
				iTween.ScaleTo (draggables [i], iTween.Hash ("scale", new Vector3 (0.0f, 0.0f, 0.0f), "time", 0.2f, "easetype", iTween.EaseType.easeInBounce));
			}
	}
}
