using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Trash : MonoBehaviour {
	public string trashType = "waste";

	Sprite spriteTrashClose;
	Sprite spriteTrashOpen;
	GameObject particleEffect;

	void Awake(){
		spriteTrashClose = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_close");
		spriteTrashOpen = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_open");
		particleEffect = Resources.Load<GameObject>
			("Trash/ParticleSystemTrash");
	}

	public void Open(){
		GetComponent<SpriteRenderer> ().sprite = spriteTrashOpen;
	}

	public void Close(){
		GetComponent<SpriteRenderer> ().sprite = spriteTrashClose;
	}

	public void MakeParticleEffect(){
		if (this.particleEffect) {
			Vector3 posParticle = transform.position;
			posParticle.y += 1;
			posParticle.z += 1;
			GameObject particleClone = Instantiate (particleEffect, posParticle, Quaternion.Euler(new Vector3(-90, 0, 0))) as GameObject;
			Destroy (particleClone, 2); 
		}
	}
}
