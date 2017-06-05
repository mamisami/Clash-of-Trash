using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Spawn Manager is the Game Object which do the spawn of wastes
/// </summary>
public class SpawnManager : NetworkBehaviour {
	static Quaternion DRAGGABLE_ROTATION = Quaternion.Euler(0, 0,0);

	//Time control
	float time = 0.0f;
	float timeOffset = 1.0f;

	/// <summary>
	/// Array of all the draggables of the current game
	/// </summary>
	public GameObject[] draggables;
	/// <summary>
	/// Array with the time when to spawn a draggable
	/// </summary>
	public double[] draggablesSpawnTime;

	/// <summary>
	/// All draggables's prefab available are stored in this array
	/// </summary>
	private GameObject[] draggablesPrefabs;

	void Start () {
		int nbDragabbles = Global.draggablesCoordinates.Length;// - (Global.level==2 ? 6 : 0);

		draggables = new GameObject[Global.draggablesCoordinates.Length];
		draggablesSpawnTime = new double[nbDragabbles];

		//Load all draggables's prefab
		draggablesPrefabs = Resources.LoadAll<GameObject>(Global.WASTES_PATH);

		Random.InitState ((int)System.DateTime.Now.Ticks);

		//Spawn all draggables
		for (int i = 0; i < nbDragabbles; i++)
			makeSpawn (i);
	}

	void Update () {
		time += Time.deltaTime;

		if (time > timeOffset) {
			timeOffset++;

			//Each time offset explore the draggable spawn time array
			for (int i = 0; i < draggablesSpawnTime.Length; i++) {
				if (draggablesSpawnTime[i] != -1.0) {
					if (draggablesSpawnTime[i] < time && Global.isStart)
						makeSpawn(i); //If it's time to spawn a draggable, make it
				}
			}
		}
	}

	/// <summary>
	/// Call the spawn of a draggable
	/// The time of spawn is random time between to var defined in Global class
	/// </summary>
	/// <param name="draggableID">draggable Id</param>
	public void spawnDraggable(int draggableID) {
		draggablesSpawnTime[draggableID] = time + Random.Range (Global.SPAWN_TIME_MIN, Global.SPAWN_TIME_MAX+1);
	}

	/// <summary>
	/// Spawn a draggable
	/// </summary>
	/// <param name="draggableID">The draggable Id</param>
	private void makeSpawn(int draggableID) {
		draggablesSpawnTime[draggableID] = -1.0; //Delete the spawn time of the draggable

		//Create the draggable and spawn it on the network
		GameObject draggablePrefab = draggablesPrefabs [Random.Range (0, draggablesPrefabs.Length)];
		GameObject draggableGameObject = Instantiate (draggablePrefab, Global.draggablesCoordinates[draggableID], DRAGGABLE_ROTATION) as GameObject;
		NetworkServer.Spawn (draggableGameObject);

		draggables [draggableID] = draggableGameObject;
		draggableGameObject.GetComponent<Draggable> ().realName = draggableID.ToString();
	}

	/// <summary>
	/// Hide all draggables
	/// </summary>
	public void HideAll(){
		for (int i = 0; i < draggables.Length; i++)
			if (draggables [i]) {
				iTween.ScaleTo (draggables [i], iTween.Hash ("scale", new Vector3 (0.0f, 0.0f, 0.0f), "time", 0.2f, "easetype", iTween.EaseType.easeInBounce));
			}
	}
}
