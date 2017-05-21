﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEditor;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

// Draggable object, when collising with another object, destroy
public class Draggable : NetworkBehaviour {

	public int wastePts = 0;
	public int aluPts = 0;
	public int compostPts = 0;
	public int glassPts = 0;
	public int paperPts = 0;
	public int petPts = 0;

	private int[] pts = new int[6];
	public string bestTrashSprite;

	PlayerController player;

	Trash trash = null;

	Vector3 screenPoint;
	Vector3 offset;

	Vector3 normalScale = new Vector3(1f,1f,1f);

	[HideInInspector]
	[SyncVar(hook="OnNameChange")]
	public string realName = "";

	void setPlayer() {
		if (player == null) {
			GameObject localPlayerObject = GameObject.FindGameObjectWithTag ("LocalPlayer");
			if (localPlayerObject != null)
				player = localPlayerObject.GetComponent<PlayerController> ();
		}
	}

	// Use this for initialization
	void Start () {
		setPlayer ();

		pts [(int)ClassificationType.Waste] = wastePts;
		pts [(int)ClassificationType.Alu] = aluPts;
		pts [(int)ClassificationType.Compost] = compostPts;
		pts [(int)ClassificationType.Glass] = glassPts;
		pts [(int)ClassificationType.Paper] = paperPts;
		pts [(int)ClassificationType.Pet] = petPts;

		UpdateBestTrashSprite ();

		normalScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3 (0f, 0f, 0f);
		iTween.ScaleTo(gameObject, iTween.Hash("scale", normalScale,"time",1f,"easetype", iTween.EaseType.easeOutElastic));
	}

	void UpdateBestTrashSprite(){
		int best = 0;

		// Find best classifcationType
		for(int i = 1; i<pts.Length;i++){
			if(pts[i] > pts[best])
				best = i;
		}

		GameObject[] trashes = Resources.LoadAll<GameObject>("Trash");

		// Find prefab for best trashes and get sprite
		foreach(GameObject trash in trashes){
			Trash t = trash.GetComponent<Trash>();
			if(t && (int)t.trashType == best)
				this.bestTrashSprite = t.GetComponent<SpriteRenderer>().sprite.name;
		}	
	}

	void OnDestroy() {
		if (trash != null)
			trash.Close();
	}

	private void OnNameChange(string value) {
		this.name = value;
	}

	void OnMouseDown(){
		setPlayer ();

		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		// Anim to grow up
		iTween.ScaleTo(gameObject, iTween.Hash("scale",normalScale + normalScale*0.2f,"time",0.2f,"easetype", iTween.EaseType.easeOutBack));
	}

	void OnMouseDrag(){
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

		move (cursorPosition);
	}

	int GetPoints() {
		if (this.trash != null)
			return pts [(int)this.trash.trashType];

		return 0;
	}

	void OnMouseUp(){
		// Anim to grow down
		iTween.ScaleTo(gameObject, iTween.Hash("scale",normalScale,"time",0.2f,"easetype", iTween.EaseType.easeOutBack));

		if (isObjectInTrash () && !trash.IsInTruckBar()) {
			trash.Close ();

			int pts = GetPoints();
			release (pts);
			this.trash.MakeParticleEffect ();

			setPlayer ();
			if (pts >= 0) {
				this.trash.MakePopScoreGood (pts);
				player.addExplanation(false, GetComponent<SpriteRenderer>().sprite.name, trash.GetComponent<SpriteRenderer>().sprite.name);
			} else {
				this.trash.MakePopScoreBad (pts);
				player.addExplanation(true, GetComponent<SpriteRenderer>().sprite.name, bestTrashSprite, trash.GetComponent<SpriteRenderer>().sprite.name);
			}
		}
	}

	void OnTriggerEnter (Collider hit) {
		Trash hitTrash = hit.gameObject.GetComponent<Trash> ();

		if (hitTrash && !hitTrash.IsInTruckBar()) {
			// Already a trash, close it
			if(trash)
				trash.Close ();
			trash = hitTrash;
			trash.Open ();
		}
	}

	void OnTriggerStay (Collider hit) {
		Trash hitTrash = hit.gameObject.GetComponent<Trash> ();

		// If no other trash, it's the current trash
		if (hitTrash != null && trash == null  && !hitTrash.IsInTruckBar()) {
			hitTrash.Open ();
			trash = hitTrash;
		}
	}

	void OnTriggerExit (Collider hit) {
		Trash trashExit = hit.gameObject.GetComponent<Trash> ();

		if (trashExit && !trashExit.IsInTruckBar()) {
			trashExit.Close ();

			if (trashExit == trash)
				trash = null;
		}
	}

	bool isObjectInTrash() {
		return trash != null;
	}

	/** MultiPlayer Functions **/
	void move(Vector3 newPosition) {
		if (isServer) {
			transform.position = newPosition;
			return;
		}

		player.CmdMoveDraggable (int.Parse(this.name), newPosition);
	}

	void release(int pts) {
		setPlayer ();

		player.CmdAddPointToScore (pts);
		player.CmdAddWaste (trash.trashType);
		player.CmdRemoveDraggable(int.Parse(this.name));
	}
}