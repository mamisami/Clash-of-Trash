﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TruckBar : NetworkBehaviour {
	PlayerController localPlayer;
	PlayerController adversary;

	float positionX = 0;

	GameObject truck;
	GameObject emptyTrash1;
	GameObject emptyTrash2;

	Vector3 emptyTrash1Position;
	Vector3 emptyTrash2Position;
	Vector3 emptyTrashScale;

	GameObject trashSlot1; //The one is for local user
	GameObject trashSlot2; //The other for the adversary or the second on single player mode

	int indexTrash = 1;

	float timeFactoryToTrash = Global.TRUCK_BAR_TIME_TO_FACTORY;


	// For call PopScore of trash in x seconds;
	Trash t;
	int score = 100;

	void setPlayers() {
		if (localPlayer == null) {
			GameObject localPlayerObject = GameObject.FindGameObjectWithTag ("LocalPlayer");
			if (localPlayerObject != null)
				localPlayer = localPlayerObject.GetComponent<PlayerController>();
		}

		if (adversary == null) {
			GameObject adversaryObject = GameObject.FindGameObjectWithTag ("Player");
			if (adversaryObject != null)
				adversary = adversaryObject.GetComponent<PlayerController>();
		}
	}

	// Use this for initialization
	void Start () {
		setPlayers ();

		truck = transform.Find ("Truck").gameObject;
		positionX = truck.transform.position.x;

		// Empty trashes
		emptyTrash1 = transform.Find ("trash_empty1").gameObject;
		emptyTrash1Position = emptyTrash1.transform.position;
		emptyTrash2 = transform.Find ("trash_empty2").gameObject;
		emptyTrash2Position = emptyTrash2.transform.position;
		emptyTrashScale = emptyTrash1.transform.lossyScale;

		// Init pos truck
		Vector3 pos = truck.transform.position;
		//pos.x += 1.2f;
		truck.transform.position = pos;
		Quaternion rot = truck.transform.rotation;
		rot.y = 180f;
		truck.transform.rotation = rot;

		MoveTruckToFactory ();
	}

	void MoveTruckToTrash(){
		iTween.MoveTo(truck,  iTween.Hash("x", positionX,"time",timeFactoryToTrash,"easetype", iTween.EaseType.linear,
			"oncompletetarget" , this.gameObject,
			"oncomplete", "TruckMovedToTrash"));
		iTween.RotateTo(truck,  iTween.Hash("y", 0f,"time",1f,"easetype", iTween.EaseType.linear));

	}

	void TruckMovedToTrash(){
		// Empty 
		EmptyTrash (trashSlot1);
		EmptyTrash (trashSlot2, 0.02f);
		trashSlot1 = null;
		trashSlot2 = null;

		this.MoveTruckToFactory ();
	}

	void EmptyTrash(GameObject go, float popScoreTime = 0.0f){			
		if (!go)
			return;
		
		Trash trash = go.GetComponent<Trash> ();

		if (!trash)
			return;

		// Replace Trash
		trash.ReplaceTrash ();

		if (Global.isSinglePlayer || (!Global.isSinglePlayer && go == trashSlot1)) {
			// TODO: add score
			score = 100;
			t = trash;

			Invoke ("MakePopScoreGoodTrash", popScoreTime);
		}

		if (isServer) {
			setPlayers ();	

			if (Global.isSinglePlayer || (!Global.isSinglePlayer && go == trashSlot1))
				this.localPlayer.CmdAddPointToScore (score);
			else
				this.adversary.CmdAddPointToScore (score);
		}
	}

	void MakePopScoreGoodTrash(){
		t.MakePopScoreGood (score);
	}

	void MoveTruckToFactory(){
		iTween.MoveTo(truck,  iTween.Hash("x", positionX-1.6f,"time",timeFactoryToTrash,"easetype", iTween.EaseType.linear,
			"oncompletetarget" , this.gameObject,
			"oncomplete", "MoveTruckToTrash"));
		iTween.RotateTo(truck,  iTween.Hash("y", 180f,"time",1f,"easetype", iTween.EaseType.linear));
	}

	public void Open() {
		iTween.FadeTo(gameObject,  iTween.Hash("alpha", 0.5f,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}

	public void Close() {
		iTween.FadeTo(gameObject,  iTween.Hash("alpha", 1f,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}

	// 0 : auto
	public void PlaceTrash(GameObject trash, Trash tr){
		int num;

		if (Global.isSinglePlayer) {
			if (trashSlot1 == null)
				num = 1;
			else if (trashSlot2 == null)
				num = 2;
			else {
				tr.ReplaceTrash ();

				return;
			}
		} else {
			num = 1;

			if (trashSlot1 != null) {
				tr.ReplaceTrash ();

				return;
			}
		}
		
		Vector3 pos = emptyTrash1Position;

		if (num == 2) {
			pos = emptyTrash2Position;

			// Update current trash 1
			if (trashSlot2 && trashSlot2 != trash/* Not the same */)
				trashSlot2.GetComponent<Trash>().ReplaceTrash ();
			trashSlot2 = trash;
		} else {

			// Update current trash 2
			if (trashSlot1 && trashSlot1 != trash)
				trashSlot1.GetComponent<Trash>().ReplaceTrash ();
			trashSlot1 = trash;
		}

		iTween.ScaleTo(trash,  iTween.Hash("scale", emptyTrashScale, "time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
		iTween.MoveTo(trash,  iTween.Hash("position", pos, "time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}		
}
