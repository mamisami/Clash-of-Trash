using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Assertions;

/// <summary>
/// Manage network connection for multiplayer
/// </summary>
public class CoT_NetworkManager : NetworkManager {
	public NetworkDiscovery discovery;

	public CountdownTimer timer;

	public Text txtAdvSearch;

	public bool isServer = false;

	private int countPlayers = 0;

	public Text countText;

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

			//Create the trash manager and spawn it on the client
			Object trashControllerPrefab = Resources.Load ("Prefabs/TrashManager", typeof(GameObject));
			GameObject trashControllerObject = Instantiate(trashControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			NetworkServer.Spawn (trashControllerObject);

			//Create the spawn manager (only on the server)
			Object spawnManagerPrefab = Resources.Load ("Prefabs/SpawnManager", typeof(GameObject));
			Instantiate(spawnManagerPrefab, Vector3.zero, Quaternion.identity);

			//If game world 2, create de truckbar and spawn it on the client
			if (Global.level == 2) {
				Object truckBarPrefab = Resources.Load ("Prefabs/TruckBar", typeof(GameObject));
				GameObject truckBarObject = Instantiate(truckBarPrefab, new Vector3(-2.15f, 23.90f, 0f), Quaternion.identity) as GameObject;
				NetworkServer.Spawn (truckBarObject);
			}

			startGame();
		}
	}

	/// <summary>
	/// Start the game
	/// </summary>
	private void startGame() {
		txtAdvSearch.enabled = false; //Hide adversary search string
		Global.isStart = true;
	}

	public override void OnClientConnect(NetworkConnection conn) {
		base.OnClientConnect (conn);

		//If the client connect start the game (only on the client, the server start at the same time but from the OnServerConnect function)
		if (!isServer)
			startGame();
	}

	public override void OnStartHost()
	{
		//When the game become a server start the discovery broadcast
		if (!Global.isSinglePlayer) {
			discovery.Initialize ();
			discovery.StartAsServer ();
		}
	}

	public override void OnStopClient() {
		base.OnStopClient ();

		clearGame ();
	}

	public override void OnStopHost() {
		base.OnStopHost ();

		clearGame ();
	}

	public override void OnStopServer() {
		base.OnStopServer ();

		clearGame ();
	}

	/// <summary>
	/// Stop the game and clear all game object
	/// </summary>
	private void clearGame() {
		Global.isStart = false;
		this.maxConnections = 2; 
		countPlayers = 0;

		GameObject spawnManager = GameObject.FindWithTag ("SpawnManager");
		if (spawnManager)
			Destroy (spawnManager);

		//Stop the broadcast of the NetworkDiscovery (free the port)
		GameObject networkDiscovery = GameObject.FindWithTag ("NetworkDiscovery");
		if (networkDiscovery) {
			try {
				networkDiscovery.GetComponent<CoT_NetworkDiscovery>().StopBroadcast();
			} catch {}

			Destroy (networkDiscovery);
		}

		SceneManager.LoadScene("Menu");
	}
}