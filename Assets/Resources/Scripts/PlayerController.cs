using System.Collections;
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
		public bool isError;
		public string waste;
		public string badTrash;
		public string goodTrash;

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

	public void generateExplanationScrollView(){
		GameObject content = GameObject.Find("/Canvas/Pause/Scroll View/Viewport/Content");
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

		foreach (KeyValuePair<string, Explanation> explKeyVal in explanations) {
			Explanation exp = explKeyVal.Value;
			GameObject go = goodRow;
			if (exp.isError)
				go = badRow;
			
			GameObject row = Instantiate (go, content.transform.position, Quaternion.identity, content.transform);
			Vector3 locPos = row.transform.localPosition;
			locPos.y -= rowHeight * i;
			row.transform.localPosition = locPos;
			row.transform.FindChild ("ImageWaste").GetComponent<Image>().sprite = 
				Resources.Load<Sprite> ("Sprites/Waste/" + exp.waste);
			row.transform.FindChild ("ImageTrash1").GetComponent<Image>().sprite = 
				stringTrashToSprite (exp.goodTrash);
			if (exp.isError) {
				row.transform.FindChild ("ImageTrash2").GetComponent<Image>().sprite = 
					stringTrashToSprite (exp.badTrash);
			}

			i++;
		}
	}

	private Sprite stringTrashToSprite(string str){
		Debug.Log ("Sprites/Trash/" + str.Split ('_') [0] + "/" + str);
		return Resources.Load<Sprite> ("Sprites/Trash/" + str.Split ('_') [0] + "/" + str);
	}

	public void addExplanation(bool isError, string waste, string goodTrash, string badTrash = ""){
		(new Explanation (isError, waste, goodTrash, badTrash)).addTo (this.explanations);
		foreach (KeyValuePair<string, Explanation> expl in explanations) {
			Debug.Log (expl.Key);
		}
		//generateExplanationScrollView ();
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

		OnScoreChange(0);
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
			txtScore.text = "    " + localPlayerScore + " pts ";
		else
			txtScore.text = "Toi : " + localPlayerScore + "pts\nAdv : " + adversaryScore + " pts ";
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

	public void StopAll(){


	}
}