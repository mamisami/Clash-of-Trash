using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	[SyncVar(hook="OnScoreChange")]
	public int score = 0;

	Text txtScore;
	CoT_NetworkManager networkManager;

	// Use this for initialization
	void Start () {
		txtScore = GameObject.FindWithTag ("TxtScore").GetComponent<Text> ();
		networkManager = GameObject.FindWithTag ("NetworkManager").GetComponent<CoT_NetworkManager> ();
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

	void destroyDraggable(string draggableName) {
		GameObject draggable = GameObject.Find(draggableName);
		Destroy (draggable);
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
	public void CmdMoveDraggable(string name, Vector3 position) {
		//GameObject draggable = FindDraggableWithRealName (name);
		GameObject draggable = GameObject.Find(name);
		if (draggable != null)
			draggable.transform.position = position;
	}

	[Command]
	public void CmdUpdateScore(ClassificationType classificationResult, string draggableName) {
		score += (int)classificationResult;
		destroyDraggable (draggableName);
	}
}
