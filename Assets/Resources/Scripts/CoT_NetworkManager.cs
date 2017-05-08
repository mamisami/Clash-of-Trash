using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

		if (countPlayers < 2 && !Constant.IS_SINGLE_PLAYER) {
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

	public override void OnServerDisconnect(NetworkConnection conn) {
		Destroy (GameObject.FindWithTag ("Timer"));

		this.maxConnections = 2; 

		this.StopHost ();
		countPlayers = 0;

		base.OnServerDisconnect (conn);
	}

	public override void OnStartHost()
	{
		if (!Constant.IS_SINGLE_PLAYER) {
			discovery.Initialize ();
			discovery.StartAsServer ();
		}
	}

	public override void OnStopClient()
	{
		//discovery.StopBroadcast();
	}

	/*
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId) {
		base.OnServerAddPlayer(conn, playerControllerId);
		playerControllers.AddRange (conn.playerControllers);
	}
	*/
}