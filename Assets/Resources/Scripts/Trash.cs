using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Trash : MonoBehaviour {
	public string trashType = "waste";

	Sprite spriteTrashClose;
	Sprite spriteTrashOpen;

	void Awake(){
		spriteTrashClose = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_close");
		spriteTrashOpen = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_open");
	}

	public void Open(){
		GetComponent<SpriteRenderer> ().sprite = spriteTrashOpen;
	}

	public void Close(){
		GetComponent<SpriteRenderer> ().sprite = spriteTrashClose;
	}
}
