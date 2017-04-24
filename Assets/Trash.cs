using UnityEngine;
using System.Collections;

public class Trash : MonoBehaviour {
	public string trashType = "waste";

	Sprite spriteTrashClose;
	Sprite spriteTrashOpen;

	public enum Direction {North, East, South, West};

	public Direction test;
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
