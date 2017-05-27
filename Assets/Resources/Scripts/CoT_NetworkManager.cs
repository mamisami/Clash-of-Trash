using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Assertions;

public class CoT_NetworkManager : NetworkManager {
	public NetworkDiscovery discovery;

	public CountdownTimer timer;

	public Text txtAdvSearch;

	public bool isServer = false;

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

		if (countPlayers < 2 && !Global.isSinglePlayer) {
			//countText.text = "Wait for one more player (" + countPlayers + ")";
		} else { 
			this.maxConnections = -1; 
			if(!Global.isSinglePlayer)
				discovery.StopBroadcast ();

			Object trashControllerPrefab = Resources.Load ("Prefabs/TrashManager", typeof(GameObject));
			GameObject trashControllerObject = Instantiate(trashControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			NetworkServer.Spawn (trashControllerObject);

			Object spawnManagerPrefab = Resources.Load ("Prefabs/SpawnManager", typeof(GameObject));
			Instantiate(spawnManagerPrefab, Vector3.zero, Quaternion.identity);

			if (Global.level == 2) {
				Object truckBarPrefab = Resources.Load ("Prefabs/TruckBar", typeof(GameObject));
				GameObject truckBarObject = Instantiate(truckBarPrefab, new Vector3(-2.15f, 23.90f, 0f), Quaternion.identity) as GameObject;
				NetworkServer.Spawn (truckBarObject);
			}

			startGame();
		}
	}

	private void startGame() {
		txtAdvSearch.enabled = false;
		Global.isStart = true;
	}

	public override void OnClientConnect(NetworkConnection conn) {
		base.OnClientConnect (conn);

		if (!isServer)
			startGame();
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
		Global.isStart = false;
		
		GameObject spawnManager = GameObject.FindWithTag ("SpawnManager");
		if (spawnManager)
			Destroy (spawnManager);

		GameObject networkDiscovery = GameObject.FindWithTag ("NetworkDiscovery");
		if (networkDiscovery)
			Destroy (networkDiscovery);

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