using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// Script attach to each waste
/// </summary>
public class Draggable : NetworkBehaviour {

	//Describe pts per trash
	public int wastePts = 0;
	public int aluPts = 0;
	public int compostPts = 0;
	public int glassPts = 0;
	public int paperPts = 0;
	public int petPts = 0;

	/// <summary>
	/// Contains all pts per trash (view each trash id in the ClassificationType class)
	/// </summary>
	private int[] pts = new int[6];

	//Best trash (trash with the most of points)
	public string bestTrashSprite;
	public int bestTrashPts;

	PlayerController player;

	Vector3 screenPoint;
	Vector3 offset;

	Vector3 normalScale = new Vector3(1f,1f,1f);

	//Name for the synchronosation between the players
	[HideInInspector]
	[SyncVar(hook="OnNameChange")]
	public string realName = "";

	/// <summary>
	/// Set the player object if is null
	/// </summary>
	void setPlayer() {
		if (player == null) {
			GameObject localPlayerObject = GameObject.FindGameObjectWithTag ("LocalPlayer");
			if (localPlayerObject != null)
				player = localPlayerObject.GetComponent<PlayerController> ();
		}
	}

	void Start () {
		setPlayer ();

		//Set the pts array
		pts [(int)ClassificationType.Waste] = wastePts;
		pts [(int)ClassificationType.Alu] = aluPts;
		pts [(int)ClassificationType.Compost] = compostPts;
		pts [(int)ClassificationType.Glass] = glassPts;
		pts [(int)ClassificationType.Paper] = paperPts;
		pts [(int)ClassificationType.Pet] = petPts;

		//Get the best trash
		UpdateBestTrashSprite ();

		normalScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3 (0f, 0f, 0f);
		iTween.ScaleTo(gameObject, iTween.Hash("scale", normalScale,"time",1f,"easetype", iTween.EaseType.easeOutElastic));
	}

	/// <summary>
	/// Gest the best trash (trash with the most of points)
	/// </summary>
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
			if(t && (int)t.trashType == best) {
				this.bestTrashSprite = t.GetComponent<SpriteRenderer>().sprite.name;
				this.bestTrashPts = pts[best];
			}
		}	
	}

	/// <summary>
	/// On the realName var chage call this function for set the name (used for move the gameobject and destroy it)
	/// </summary>
	/// <param name="value">The new name</param>
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
		//Calc the point of the finger (finger / mouse = same)
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

		move (cursorPosition);
	}

	/// <summary>
	/// Get the points obtain for the current selected trash
	/// </summary>
	/// <returns>The points. If no trash selected return 0</returns>
	int GetPoints() {
		if (isObjectInTrash())
			return pts [(int)this.player.trashToDrag.trashType];

		return 0;
	}

	void OnMouseUp(){
		// Anim to grow down
		iTween.ScaleTo(gameObject, iTween.Hash("scale",normalScale,"time",0.2f,"easetype", iTween.EaseType.easeOutBack));

		if (isObjectInTrash () && !this.player.trashToDrag.IsInTruckBar()) {
			player.trashToDrag.Close (); //Close the trash

			//Get the points
			int pts = GetPoints();

			//Pop the score on the trash and add in the explanation panel
			setPlayer ();
			if (pts >= 0) {
				player.trashToDrag.MakePopScoreGood (pts);

				if (bestTrashPts > pts)
					player.addExplanation(true, GetComponent<SpriteRenderer>().sprite.name, bestTrashSprite, player.trashToDrag.GetComponent<SpriteRenderer>().sprite.name, true); //Better choice
				else
					player.addExplanation(false, GetComponent<SpriteRenderer>().sprite.name, player.trashToDrag.GetComponent<SpriteRenderer>().sprite.name);
			} else {
				player.trashToDrag.MakePopScoreBad (pts);
				player.addExplanation(true, GetComponent<SpriteRenderer>().sprite.name, bestTrashSprite, player.trashToDrag.GetComponent<SpriteRenderer>().sprite.name);
			}

			//Make the effect, add pts and destroy the object
			this.player.trashToDrag.MakeParticleEffect ();
			release (pts);
		}
	}

	/// <summary>
	/// Test if the object (mouse or finger) is over a trash
	/// </summary>
	/// <returns>true if it is, false otherwise</returns>
	bool isObjectInTrash() {
		return player.trashToDrag != null;
	}

	/** MultiPlayer Functions **/

	/// <summary>
	/// Move the draggable
	/// </summary>
	/// <param name="newPosition">The position to move</param>
	void move(Vector3 newPosition) {
		///If we are on the server we just need to move the object (the synchronazed rigidbody do the synchronization with the client)
		///But if we are on the client we call the server for move the draggable
		if (isServer) {
			transform.position = newPosition;
			return;
		}

		//Call the server for move the draggable
		player.CmdMoveDraggable (int.Parse(this.name), newPosition);
	}

	/// <summary>
	/// Release the draggable
	/// </summary>
	/// <param name="pts">Points to add to the player</param>
	void release(int pts) {
		setPlayer ();

		player.CmdAddPointToScore (pts); //Add points
		player.CmdAddWaste (player.trashToDrag.trashType); //Add the object to the trash counter
		player.CmdRemoveDraggable(int.Parse(this.name)); //Call the server for destroy the draggable
	}
}