using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// Player controller (player object in multiplayer)
/// </summary>
public class PlayerController : NetworkBehaviour {
	/// <summary>
	/// Score var with a hook on it for display all changes
	/// </summary>
	[SyncVar(hook="OnScoreChange")]
	public int score = 0;

	Text txtScore;
	SpawnManager spawnManager;
	TrashManager trashManager;

	GameObject truckBarTrash1;
	GameObject truckBarTrash2;

	public Trash trashToDrag;

	private SortedList<string, Explanation> explanations = new SortedList<string, Explanation>();

	/// <summary>
	/// Represent an explanation shown in the finish scrollview
	/// </summary>
	public class Explanation {
		public bool isError;
		public bool betterTrash; //Describe if a better trash is available if it's not an error
		public string waste;
		public string badTrash;
		public string goodTrash;

		public Explanation(bool isError, string waste, string goodTrash, string badTrash = "", bool betterTrash = false){
			this.isError = isError;
			this.waste = waste;
			this.badTrash = badTrash;
			this.goodTrash = goodTrash;
			this.betterTrash = betterTrash;
		}

		/// <summary>
		/// Get the key for the array insertion (first two char are for the order of display)
		/// </summary>
		/// <returns>A string with the key</returns>
		public string getKey(){
			return (this.isError ? "0" : "1") + (this.betterTrash ? "1" : "0") + this.waste + this.badTrash + this.goodTrash;
		}
			
		/// <summary>
		/// Add the explanation to a list
		/// </summary>
		/// <param name="list">The list on which we add the explanation</param>
		public void addTo(SortedList<string, Explanation> list){
			string key = getKey ();
			if (!list.ContainsKey (key)) 
				list.Add (key, this);
		}
	}

	/// <summary>
	/// Generate the explanation scroll view at the end of the game
	/// </summary>
	public void generateExplanationScrollView(){
		GameObject content = GameObject.Find("/Canvas/Finish/Panel/Scroll View/Viewport/Content");
		GameObject goodRow = Resources.Load<GameObject> ("ScrollView/GoodRow");
		GameObject badRow = Resources.Load<GameObject> ("ScrollView/BadRow");

		if (!content)
			return;
		
		// Remove all raws
		var children = new List<GameObject>();
		foreach (Transform child in content.transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));

		Vector2 sd = content.GetComponent<RectTransform> ().sizeDelta;
		float rowHeight = goodRow.transform.GetComponent<RectTransform> ().sizeDelta.y*goodRow.transform.localScale.y;
		sd.y = rowHeight * explanations.Count + rowHeight / 4;
		content.GetComponent<RectTransform> ().sizeDelta = sd;
		int i = 0;

		//Generate all explanation rows
		foreach (KeyValuePair<string, Explanation> explKeyVal in explanations) {
			Explanation exp = explKeyVal.Value;
			GameObject go = goodRow;
			if (exp.isError)
				go = badRow;
			
			//Generate the row
			GameObject row = Instantiate (go, content.transform.position, Quaternion.identity, content.transform);
			Vector3 locPos = row.transform.localPosition;
			locPos.y -= rowHeight * i;
			row.transform.localPosition = locPos;
			row.transform.FindChild ("ImageWaste").GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Waste/" + exp.waste);
			row.transform.FindChild ("ImageTrash1").GetComponent<Image>().sprite = stringTrashToSprite (exp.goodTrash);
			if (exp.isError) {
				row.transform.FindChild ("ImageTrash2").GetComponent<Image>().sprite = stringTrashToSprite (exp.badTrash);

				//If a better trash is possible, display it
				if (exp.betterTrash) {
					row.transform.FindChild ("ImageSignal").GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Symbols/best");
					row.transform.FindChild ("Text1").GetComponent<Text>().text = "peut aller dans";
					row.transform.FindChild ("Text2").GetComponent<Text>().text = "va mieux dans";
				}
			}

			i++;
		}
	}

	/// <summary>
	/// Call to have a trash sprite
	/// </summary>
	/// <param name="str">Trash name</param>
	/// <returns>Trash sprite</returns>
	private Sprite stringTrashToSprite(string str){
		Debug.Log ("Sprites/Trash/" + str.Split ('_') [0] + "/" + str);
		return Resources.Load<Sprite> ("Sprites/Trash/" + str.Split ('_') [0] + "/" + str);
	}

	/// <summary>
	/// Add an explanation to the finish scroll view
	/// </summary>
	/// <param name="isError">Is the user is wrong</param>
	/// <param name="waste">Name of the waste</param>
	/// <param name="goodTrash">What is the goode trash</param>
	/// <param name="badTrash">What is the bad trash (or the less good)</param>
	/// <param name="betterTrash">Bool which say that the user haven't wrong but there is a better choice</param>
	public void addExplanation(bool isError, string waste, string goodTrash, string badTrash = "", bool betterTrash = false){
		(new Explanation (isError, waste, goodTrash, badTrash, betterTrash)).addTo (this.explanations);
		foreach (KeyValuePair<string, Explanation> expl in explanations) {
			Debug.Log (expl.Key);
		}
	}

	/// <summary>
	/// Set managers game object if there are null
	/// </summary>
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

	void Start () {
		txtScore = GameObject.FindWithTag ("TxtScore").GetComponent<Text> ();
	
		//Init the score
		OnScoreChange(0);
	}

	/// <summary>
	/// Tag the local player like that which means that the adversary have just the tag "player"
	/// </summary>
	override public void OnStartLocalPlayer() {
		this.tag = "LocalPlayer";
	}

	/// <summary>
	/// Called when the score var change
	/// </summary>
	/// <param name="value">The new score value</param>
	void OnScoreChange(int value) {
		int localPlayerScore;
		int adversaryScore = 0;

		if (NetworkManager.singleton.IsClientConnected ())
			score = value;

		//Get the score of all players
		//Check if we are on the local player object
		if (this.tag == "LocalPlayer") {
			localPlayerScore = value;
			if (!Global.isSinglePlayer) {
				GameObject adv = GameObject.FindWithTag ("Player");
				if (adv)
					adversaryScore = adv.GetComponent<PlayerController> ().score;
				else
					adversaryScore = 0;
			}
		} else {
			localPlayerScore = GameObject.FindWithTag ("LocalPlayer").GetComponent<PlayerController> ().score;
			if (!Global.isSinglePlayer)
				adversaryScore = value;
		}

		//Show the score
		if (Global.isSinglePlayer)
			txtScore.text = "    " + localPlayerScore + " pts";
		else
			txtScore.text = "Toi : " + localPlayerScore + " pts\nAdv : " + adversaryScore + " pts";
	}

	/// <summary>
	/// EXECUTE ONLY ON THE SERVER - Add points to score
	/// </summary>
	/// <param name="point">Points to add (can be negative)</param>
	[Command]
	public void CmdAddPointToScore(int point) {
		this.score += (int)point;
	}

	/// <summary>
	/// EXECUTE ONLY ON THE SERVER - Add a waste to the trash
	/// </summary>
	/// <param name="type">Trash where to add the waste</param>
	[Command]
	public void CmdAddWaste(ClassificationType type) {
		setManagers();
		
		trashManager.addWaste(type);
	}

	/// <summary>
	/// EXECUTE ONLY ON THE SERVER - Move a draggable
	/// </summary>
	/// <param name="id">Id of the draggable in the spawnmanager array (id is the name of the object too)</param>
	/// <param name="newPosition">Position where to move the draggable</param>
	[Command]
	public void CmdMoveDraggable(int id, Vector3 newPosition) {
		setManagers();

		spawnManager.draggables[id].transform.position = newPosition;
	}

	/// <summary>
	/// EXECUTE ONLY ON THE SERVER - Destroy a draggable
	/// </summary>
	/// <param name="draggableID">Id of the draggable in the spawnmanager array (id is the name of the object too)</param>
    [Command]
	public void CmdRemoveDraggable(int draggableID) {
        setManagers();

		//Destroy the draggable and say to the spawnmanager that we need a new one
		Destroy (spawnManager.draggables[draggableID]);
		spawnManager.spawnDraggable (draggableID);
	}

	/// <summary>
	/// EXECUTE ONLY ON THE SERVER - Add a trash on the truck bar
	/// </summary>
	/// <param name="tag"></param>
	[Command]
	public void CmdAddTrashToTruckBar(string tag) {
		if (isLocalPlayer) {
			RpcAddTrashToTruckBar(tag);
		} else {
			AddTrashToTruckBar(tag);
		}
	}

	/// <summary>
	/// EXECUTE ONLY ON CLIENTS - Add a trash on the truck bar
	/// </summary>
	/// <param name="tag"></param>
	[ClientRpc]
	public void RpcAddTrashToTruckBar(string tag) {
		if (!isLocalPlayer) {
			AddTrashToTruckBar(tag);
		}
	}

	/// <summary>
	/// Add a trash on the truck bar
	/// </summary>
	/// <param name="tag">tag of the trash</param>
	public void AddTrashToTruckBar(string tag) {
		GameObject trashObject = GameObject.FindWithTag(tag);
		Trash trash = trashObject.GetComponent<Trash>();

		TruckBar truckBar = GameObject.FindWithTag("TruckBar").GetComponent<TruckBar>();

		//Add the trash
		trash.ShowEmptyTrash();
		truckBar.AddTrash(trashObject, trash, 2);
	}
}