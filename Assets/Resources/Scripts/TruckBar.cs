using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TruckBar : MonoBehaviour {

	float positionX = 0;

	GameObject truck;
	GameObject emptyTrash1;
	GameObject emptyTrash2;

	Vector3 emptyTrash1Position;
	Vector3 emptyTrash2Position;
	Vector3 emptyTrashScale;

	GameObject currentTrash1;
	GameObject currentTrash2;


	int indexTrash = 1;

	float timeFactoryToTrash = Global.TRUCK_BAR_TIME_TO_FACTORY;


	// For call PopScore of trash in x seconds;
	Trash t;
	int score = 100;

	PlayerController player;

	void OnPlayerConnected(NetworkPlayer player) {
		this.player =  GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();
	}

	// Use this for initialization
	void Start () {
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
		EmptyTrash (currentTrash1);
		EmptyTrash (currentTrash2, true, 0.02f);
		currentTrash1 = null;
		currentTrash2 = null;

		this.MoveTruckToFactory ();
	}

	void EmptyTrash(GameObject go, bool popScore = true, float popScoreTime = 0f){
		if (!go)
			return;
		Trash trash = go.GetComponent<Trash> ();
		if (!trash)
			return;

		// Replace Trash
		trash.ReplaceTrash ();

		if (popScore) {
			// TODO: add score
			score = 100;
			t = trash;

			Invoke("MakePopScoreGoodTrash", popScoreTime);
		}
		if(this.player == null)
			this.player =  GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();
		this.player.CmdAddPointToScore (score);
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
	public void PlaceTrash(GameObject trash, int num = 0){
		indexTrash += 1;
		if (num == 0)
			num = indexTrash % 2 + 1;
		
		Vector3 pos = emptyTrash1Position;

		if (num == 2) {
			pos = emptyTrash2Position;

			// Update current trash 1
			if (currentTrash2 && currentTrash2 != trash/* Not the same */)
				currentTrash2.GetComponent<Trash>().ReplaceTrash ();
			currentTrash2 = trash;
		} else {

			// Update current trash 2
			if (currentTrash1 && currentTrash1 != trash)
				currentTrash1.GetComponent<Trash>().ReplaceTrash ();
			currentTrash1 = trash;
		}

		iTween.ScaleTo(trash,  iTween.Hash("scale", emptyTrashScale, "time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
		iTween.MoveTo(trash,  iTween.Hash("position", pos, "time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}		
}
