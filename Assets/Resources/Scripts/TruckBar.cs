using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Truck bar game object 
/// </summary>
public class TruckBar : NetworkBehaviour {
	PlayerController localPlayer;
	PlayerController adversary;

	TrashManager trashManager;

	/// <summary>
	/// Position of the truck
	/// </summary>
	float positionX = 0;

	GameObject truck;
	GameObject emptyTrash1;
	GameObject emptyTrash2;

	Vector3 emptyTrash1Position;
	Vector3 emptyTrash2Position;
	Vector3 emptyTrashScale;

	GameObject trashSlot1; //The one is for local user
	GameObject trashSlot2; //The other for the adversary or the second on single player mode

	float timeFactoryToTrash = Global.TRUCK_BAR_TIME_TO_FACTORY;

	/// <summary>
	/// Set controllers game object if there are null
	/// </summary>
	void setControllers() {
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

		if (trashManager == null) {
			GameObject trashManagerObject = GameObject.FindGameObjectWithTag ("TrashManager");
			if (trashManagerObject != null)
				trashManager = trashManagerObject.GetComponent<TrashManager>();
		}
	}

	void Start () {
		setControllers ();

		//Get game objects
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

		//Start the truckbar
		MoveTruckToFactory ();
	}

	/// <summary>
	/// Make the animation of the truck go to the trash
	/// </summary>
	void MoveTruckToTrash(){
		iTween.MoveTo(truck,  iTween.Hash("x", positionX,"time",timeFactoryToTrash,"easetype", iTween.EaseType.linear,
			"oncompletetarget" , this.gameObject,
			"oncomplete", "TruckMovedToTrash"));
		iTween.RotateTo(truck,  iTween.Hash("y", 0f,"time",1f,"easetype", iTween.EaseType.linear));
	}

	/// <summary>
	/// Called when the truck arrive to trashes
	/// </summary>
	void TruckMovedToTrash(){
		// Empty trashes (get points too)
		EmptyTrash (trashSlot1);
		EmptyTrash (trashSlot2);
		trashSlot1 = null;
		trashSlot2 = null;

		//Go to the factory
		this.MoveTruckToFactory ();
	}

	/// <summary>
	/// Clear a trash
	/// </summary>
	/// <param name="go">Game Object of the trash</param>
	void EmptyTrash(GameObject go){	
		if (!go)
			return;
		
		Trash trash = go.GetComponent<Trash> ();

		if (!trash)
			return;

		// Replace Trash
		trash.ReplaceTrash ();
		trash.draggable = true;

		//Get points of the trash
		int score = 0; 
		try {
			setControllers ();	
			score = trashManager.getPoints(trash.trashType);
		} catch {
			
		}

		//Pop the score
		if (Global.isSinglePlayer || (!Global.isSinglePlayer && go == trashSlot1)) {
			trash.MakePopScoreGood (score);
		}

		//Add points obtained
		if (isServer) {
			if (Global.isSinglePlayer || (!Global.isSinglePlayer && go == trashSlot1))
				this.localPlayer.CmdAddPointToScore (score);
			else
				this.adversary.CmdAddPointToScore (score);

			trashManager.reinitWaste(trash.trashType);
		}
	}

	/// <summary>
	/// Make the animation of the truck go to the factory and make sound for clear indication
	/// </summary>
	void MoveTruckToFactory(){
		iTween.MoveTo(truck,  iTween.Hash("x", positionX-1.6f,"time",timeFactoryToTrash,"easetype", iTween.EaseType.linear,
			"oncompletetarget" , this.gameObject,
			"oncomplete", "MoveTruckToTrash"));
		iTween.RotateTo(truck,  iTween.Hash("y", 180f,"time",1f,"easetype", iTween.EaseType.linear));

		//play sound and destroy audio source
		AudioClip myClip = Resources.Load<AudioClip>("Music/TruckSound");
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = myClip;
		audioSource.volume = 0.2f;
		audioSource.Play();
		Destroy(audioSource, myClip.length);
	}

	/// <summary>
	/// "Open" the truckbar when a waste is in (make alpha greater)
	/// </summary>
	public void Open() {
		iTween.FadeTo(gameObject,  iTween.Hash("alpha", 0.5f,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}

	/// <summary>
	/// "Close" the truckbar when a waste is in (make alpha lower)
	/// </summary>
	public void Close() {
		iTween.FadeTo(gameObject,  iTween.Hash("alpha", 1f,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}

	/// <summary>
	/// Place a trash to a slot in the truckbar
	/// </summary>
	/// <param name="trash">Trash game object</param>
	/// <param name="tr">Trash object</param>
	public void PlaceTrash(GameObject trash, Trash tr){
		//Choose the slot (in single player -> the first free slot) (in multiplayer, on the first slot if free, the second is the slot of the adversary)
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

			setControllers();
			localPlayer.CmdAddTrashToTruckBar(trash.tag);
		}

		//Add the trash to a specified slot
		AddTrash(trash, tr, num);
	}

	/// <summary>
	/// Place the trash to a specified slot
	/// </summary>
	/// <param name="trash">Trash game object</param>
	/// <param name="tr">Trash object</param>
	/// <param name="num">Slot number</param>
	public void AddTrash(GameObject trash, Trash tr, int num){		
		tr.draggable = false;

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
