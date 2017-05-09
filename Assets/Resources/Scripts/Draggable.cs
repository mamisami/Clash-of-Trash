using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]


// Draggable object, when collising with another object, destroy
public class Draggable : NetworkBehaviour {

	PlayerController player;

	Trash trash = null;

	Vector3 screenPoint;
	Vector3 offset;

	[SyncVar(hook="OnNameChange")]
	public string realName = "";

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();

		gameObject.transform.localScale = new Vector3 (0f, 0f, 0f);
		iTween.ScaleTo(gameObject, iTween.Hash("scale",new Vector3(1f,1f,1f),"time",1f,"easetype", iTween.EaseType.easeOutElastic));
	}

	void OnDestroy() {
		if (trash != null)
			trash.Close();
	}

	private void OnNameChange(string value) {
		this.name = value;
	}

	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		// Anim to grow up
		iTween.ScaleTo(gameObject, iTween.Hash("scale",new Vector3(1.2f,1.2f,1.2f),"time",0.2f,"easetype", iTween.EaseType.easeOutBack));
	}

	void OnMouseDrag(){
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

		move (cursorPosition);
	}

	void OnMouseUp(){
		// Anim to grow down
		iTween.ScaleTo(gameObject, iTween.Hash("scale",new Vector3(1f,1f,1f),"time",0.2f,"easetype", iTween.EaseType.easeOutBack));

		if (isObjectInTrash () && !trash.IsInTruckBar()) {
			trash.Close ();

			//TODO: Determiner si le release est le bon
			release (ClassificationType.Good);
			this.trash.MakeParticleEffect ();
			this.trash.MakePopScoreGood (10); // Or MakePopScoreBad(score)

			//Destroy (gameObject);
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

	void release(ClassificationType classificationType) {
		player.CmdUpdateScore (classificationType, int.Parse(this.name));
	}
}