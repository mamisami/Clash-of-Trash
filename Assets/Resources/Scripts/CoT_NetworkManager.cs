using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Assertions;

public class CoT_NetworkManager : NetworkManager {
	private int countPlayers = 0;
	//public GameObject playerPrefab;

	public Text countText;
	/*
	public Text winText;
	public Text hitText;
	public CameraController cameraController;
	*/

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect (conn);

		countPlayers++;

		if (countPlayers < 2) 
			countText.text = "Wait for one more player (" + countPlayers + ")";
		else { 
			this.maxConnections = -1; 
			//countText.text = "";

			Object timerPrefab = Resources.Load ("Prefabs/Timer", typeof(GameObject));
			GameObject timerGameObject = Instantiate (timerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			//TimerController timer = timerGameObject.GetComponent<TimerController> ();
			//NetworkServer.Spawn (timerGameObject);
		}
	}

	public override void OnServerDisconnect(NetworkConnection conn) {
		this.StopHost ();
		countPlayers = 0;
	}
}