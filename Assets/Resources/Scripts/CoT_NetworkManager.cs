using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Assertions;

public class CoT_NetworkManager : NetworkManager {
	public NetworkDiscovery discovery;

	private int countPlayers = 0;
	//public GameObject playerPrefab;

	public Text countText;
	/*
	public Text winText;
	public Text hitText;
	public CameraController cameraController;
	*/

	//public List<UnityEngine.Networking.PlayerController> playerControllers = new List<UnityEngine.Networking.PlayerController> ();

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect (conn);

		countPlayers++;

		/*
		Object playerPrefab = Resources.Load ("Prefabs/Player", typeof(GameObject));
		GameObject playerGameObject = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		NetworkServer.AddPlayerForConnection(conn, playerGameObject, 0);
		*/

		if (countPlayers < 2 && !Global.isSinglePlayer) {
			//countText.text = "Wait for one more player (" + countPlayers + ")";
		} else { 
			this.maxConnections = -1; 
			discovery.StopBroadcast ();
			//countText.text = "";

			Object timerPrefab = Resources.Load ("Prefabs/Timer", typeof(GameObject));
			Instantiate (timerPrefab, Vector3.zero, Quaternion.identity);
			//TimerController timer = timerGameObject.GetComponent<TimerController> ();
			//NetworkServer.Spawn (timerGameObject);
		}
	}

	public override void OnStartHost()
	{
		if (!Global.isSinglePlayer) {
			discovery.Initialize ();
			discovery.StartAsServer ();
		}
	}

	public override void OnStopClient() {
		clearGame ();
	}

	public override void OnStopHost() {
		clearGame ();
	}

	public override void OnStopServer() {
		clearGame ();
	}

	private void clearGame() {
		GameObject timer = GameObject.FindWithTag ("Timer");
		if (timer)
			Destroy (timer);

		this.maxConnections = 2; 

		//this.StopHost ();
		countPlayers = 0;

		SceneManager.LoadScene("Menu");
	}

	/*
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId) {
		base.OnServerAddPlayer(conn, playerControllerId);
		playerControllers.AddRange (conn.playerControllers);
	}
	*/
}