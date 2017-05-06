﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Trash : MonoBehaviour {
	public string trashType = "waste";
	public Color colorGoodText = Color.green;
	public Color colorBadText = Color.red;


	Sprite spriteTrashClose;
	Sprite spriteTrashOpen;
	GameObject particleEffect;
	GameObject bonusText;

	void Awake(){
		spriteTrashClose = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_close");
		spriteTrashOpen = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_open");
		particleEffect = Resources.Load<GameObject>
			("Trash/ParticleSystemTrash");
		bonusText = Resources.Load<GameObject>
			("Trash/PopText3D");
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
			posParticle.y += 1.5f;
			posParticle.z += 1;
			GameObject particleClone = Instantiate (particleEffect, posParticle, Quaternion.Euler(new Vector3(-90, 0, 0))) as GameObject;
			Destroy (particleClone, 2);
		}
	}

	public void MakePopText(string text, Color color){
		Vector3 posText = transform.position;
		GameObject bonusClone = Instantiate (bonusText, posText, Quaternion.identity) as GameObject;
		Destroy (bonusClone, 3f); // Destoy 3 seconds after
		TextMesh txtMesh = bonusClone.GetComponentInChildren<TextMesh> ();
		txtMesh.alignment = TextAlignment.Center;
		txtMesh.text = text;
		txtMesh.color = color;
		posText.y += 1f;
		//posText.z += 1;
		bonusClone.transform.position = posText;
	}

	public void MakePopScoreGood(int score){
		MakePopText("+"+score, colorGoodText);
	}

	public void MakePopScoreBad(int score){
		MakePopText("-"+score, colorBadText);
	}
}
