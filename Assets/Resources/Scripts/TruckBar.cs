using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckBar : MonoBehaviour {

	float positionX = 0;
	Vector3 scale;

	GameObject truck;
	GameObject emptyTrash1;
	GameObject emptyTrash2;

	Vector3 emptyTrash1Position;
	Vector3 emptyTrash2Position;
	Vector3 emptyTrashScale;

	// Use this for initialization
	void Start () {
		truck = transform.Find ("Truck").gameObject;

		// Empty trashes
		emptyTrash1 = transform.Find ("trash_empty1").gameObject;
		emptyTrash1Position = emptyTrash1.transform.position;
		emptyTrash2 = transform.Find ("trash_empty2").gameObject;
		emptyTrash2Position = emptyTrash2.transform.position;
		emptyTrashScale = emptyTrash1.transform.lossyScale;

		positionX = truck.transform.position.x;
		scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		//iTween.MoveTo(gameObject,new Vector3(12.5f,-56,90),60);
		iTween.MoveTo(truck,  iTween.Hash("x", positionX+1.6f,"time",2,"easetype", iTween.EaseType.linear));
	}

	public void Open() {
		//iTween.ScaleTo(gameObject,  iTween.Hash("scale", 1.1F*scale,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
		iTween.FadeTo(gameObject,  iTween.Hash("alpha", 0.5f,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}

	public void Close() {
		//iTween.ScaleTo(gameObject,  iTween.Hash("scale", scale,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
		iTween.FadeTo(gameObject,  iTween.Hash("alpha", 1f,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}

	public void PlaceTrash(GameObject trash, int num = 1){
		GameObject emptyTrash = emptyTrash1;
		Vector3 pos = emptyTrash1Position;

		if (num == 2) {
			emptyTrash = emptyTrash2;
			pos = emptyTrash2Position;
		}

		trash.transform.position = pos;
		//iTween.MoveTo(trash,  iTween.Hash("position", emptyTrash.transform.position,"time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
		iTween.ScaleTo(trash,  iTween.Hash("scale", emptyTrashScale, "time",0.8f,"easetype", iTween.EaseType.easeOutExpo));
	}		
}
