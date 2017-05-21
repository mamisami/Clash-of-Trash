﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	[SyncVar(hook="OnScoreChange")]
	public int score = 0;

	Text txtScore;
	SpawnManager spawnManager;
	TrashManager trashManager;

	GameObject truckBarTrash1;
	GameObject truckBarTrash2;


	public class Explanation {
		bool isError;
		string waste;
		string badTrash;
		string goodTrash;

		public Explanation(bool isError, string waste, string goodTrash, string badTrash = ""){
			this.isError = isError;
			this.waste = waste;
			this.badTrash = badTrash;
			this.goodTrash = goodTrash;
		}

		public string getKey(){
			return (this.isError ? "0" : "1") + this.waste + this.badTrash + this.goodTrash;
		}
			
		public void addTo(SortedList<string, Explanation> list){
			string key = getKey ();
			if (!list.ContainsKey (key))
				list.Add (key, this);
		}
	}

	private SortedList<string, Explanation> explanations = new SortedList<string, Explanation>();

	public void addExplanation(bool isError, string waste, string goodTrash, string badTrash = ""){
		(new Explanation (isError, waste, badTrash, goodTrash)).addTo (this.explanations);
		Debug.Log ("LIST");
		foreach (KeyValuePair<string, Explanation> expl in explanations) {
			Debug.Log (expl.Key);
		}
	}

	public void setManagers() {
		if (spawnManager == null) {
			GameObject spawnManagerObject = GameObject.FindWithTag ("SpawnManager");
			if (spawnManagerObject != null)
				spawnManager = spawnManagerObject.GetComponent<SpawnManager> ();
		}

		if (trashManager == null) {
			GameObject trashManagerObject = GameObject.FindWithTag ("TrashManager");
			if (trashManagerObject != null)
				trashManager = trashManagerObject.GetComponent<TrashManager> ();
		}
    }

	// Use this for initialization
	void Start () {
		txtScore = GameObject.FindWithTag ("TxtScore").GetComponent<Text> ();
	}

	override public void OnStartLocalPlayer() {
		this.tag = "LocalPlayer";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnScoreChange(int value) {
		int localPlayerScore;
		int adversaryScore = 0;;

		if (NetworkManager.singleton.IsClientConnected ())
			score = value;

		if (this.tag == "LocalPlayer") {
			localPlayerScore = value;
			if (!Global.isSinglePlayer)
				adversaryScore = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().score;
		} else {
			localPlayerScore = GameObject.FindWithTag ("LocalPlayer").GetComponent<PlayerController> ().score;
			if (!Global.isSinglePlayer)
				adversaryScore = value;
		}

		if (Global.isSinglePlayer)
			txtScore.text = "Your score : " + localPlayerScore + "pts";
		else
			txtScore.text = "Your score : " + localPlayerScore + "pts\nAdversary  : " + adversaryScore + "pts";
	}



	/** Command functions **/
	/*
	GameObject FindDraggableWithRealName(string name) {
		GameObject[] draggables = GameObject.FindGameObjectsWithTag("Draggable");
		foreach (GameObject draggableObject in draggables) {
			Draggable draggable = draggableObject.GetComponent<Draggable>();
			if (draggable.realName == name) {
				return draggableObject;
			}
		}

		return null;
	}
	*/

	[Command]
	public void CmdAddPointToScore(int point) {
		this.score += (int)point;
	}

	[Command]
	public void CmdAddWaste(ClassificationType type) {
		setManagers();
		
		trashManager.addWaste(type);
	}

	[Command]
	public void CmdMoveDraggable(int id, Vector3 newPosition) {
		setManagers();

		spawnManager.draggables[id].transform.position = newPosition;
	}

    [Command]
	public void CmdRemoveDraggable(int draggableID) {
        setManagers();

		Destroy (spawnManager.draggables[draggableID]);
		spawnManager.spawnDraggable (draggableID);
	}

	[Command]
	public void CmdAddTrashToTruckBar(string tag) {
		if (isLocalPlayer) {
			RpcAddTrashToTruckBar(tag);
		} else {
			AddTrashToTruckBar(tag);
		}
	}

	[ClientRpc]
	public void RpcAddTrashToTruckBar(string tag) {
		if (!isLocalPlayer) {
			AddTrashToTruckBar(tag);
		}
	}

	public void AddTrashToTruckBar(string tag) {
		GameObject trashObject = GameObject.FindWithTag(tag);
		Trash trash = trashObject.GetComponent<Trash>();

		TruckBar truckBar = GameObject.FindWithTag("TruckBar").GetComponent<TruckBar>();

		trash.ShowEmptyTrash();
		truckBar.AddTrash(trashObject, trash, 2);
	}
}