using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	[SyncVar(hook="OnScoreChange")]
	public int score = 0;

	Text txtScore;
	TimerController timer;

	// Use this for initialization
	void Start () {
		txtScore = GameObject.FindWithTag ("TxtScore").GetComponent<Text> ();

		GameObject timerObject = GameObject.FindWithTag ("Timer");
		if (timerObject != null)
			timer = timerObject.GetComponent<TimerController> ();
	}

	override public void OnStartLocalPlayer() {
		this.tag = "LocalPlayer";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnScoreChange(int value) {
		int localPlayerScore;
		int adversaryScore;

		if (this.tag == "LocalPlayer") {
			localPlayerScore = value;
			adversaryScore = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ().score;
		} else {
			localPlayerScore = GameObject.FindWithTag ("LocalPlayer").GetComponent<PlayerController> ().score;
			adversaryScore = value;
		}

		txtScore.text = "Your score : " + localPlayerScore + "pts\nAdversary  : " + adversaryScore + "pts";
	}

	void destroyDraggable(int draggableID) {
		Destroy (timer.draggables[draggableID]);
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
	public void CmdMoveDraggable(int id, Vector3 newPosition) {
		//GameObject draggable = FindDraggableWithRealName (name);
		//GameObject draggable = GameObject.Find(name);
		//if (draggable != null)
			//draggable.transform.position = position;

		timer.draggables[id].transform.position = newPosition;
	}

	[Command]
	public void CmdUpdateScore(ClassificationType classificationResult, int draggableID) {
		score += (int)classificationResult;
		destroyDraggable (draggableID);
	}
}
