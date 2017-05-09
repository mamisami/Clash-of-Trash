﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Trash : MonoBehaviour {
	public string trashType = "waste";
	public Color colorGoodText = Color.green;
	public Color colorBadText = Color.red;
	public bool draggable = true;

	Sprite spriteTrashClose;
	Sprite spriteTrashOpen;
	GameObject particleEffect;
	GameObject bonusText;
	GameObject trashEmpty;

	SpriteRenderer spriteRenderer;

	Vector3 screenPointDrag;
	Vector3 offsetDrag;

	float startScale = 1f;
	bool moving = false;
	GameObject trashEmptyClone;


	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteTrashClose = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_close");
		spriteTrashOpen = Resources.Load<Sprite> 
			("Sprites/Trash/"+trashType+"/"+trashType+"_open");
		particleEffect = Resources.Load<GameObject>
			("Trash/ParticleSystemTrash");
		bonusText = Resources.Load<GameObject>
			("Trash/PopText3D");
		trashEmpty = Resources.Load<GameObject>
			("Trash/TrashEmpty");
		startScale = transform.localScale.x;

		draggable = Global.level == 2;

		trashEmptyClone = Instantiate (trashEmpty, transform.position, transform.rotation) as GameObject;
		trashEmptyClone.transform.localScale = transform.localScale;
		//trashEmptyClone.GetComponent<Renderer>().enabled = false;
		iTween.FadeTo(trashEmptyClone, iTween.Hash("alpha", 0f, "time",0.2f,"easetype", iTween.EaseType.linear));
		iTween.ScaleTo (trashEmptyClone, iTween.Hash ("scale", new Vector3(startScale*0.5f, startScale*0.5f, startScale*0.5f), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));

	}

	public void Open(){
		if(!moving)
			spriteRenderer.sprite = spriteTrashOpen;
	}

	public void Close(){
		spriteRenderer.sprite = spriteTrashClose;
	}

	public void MakeParticleEffect(){
		if (this.particleEffect) {
			Vector3 posParticle = transform.position;
			posParticle.y += 1.5f;
			posParticle.z += 1;
			GameObject particleClone = Instantiate (particleEffect, posParticle, Quaternion.Euler(new Vector3(-90, 0, 0))) as GameObject;
			Destroy (particleClone, 2);
		}

		// Make rotation effect
		Quaternion v = gameObject.transform.rotation;
		v.z = -0.1f;
		transform.rotation = v;
		iTween.RotateTo(gameObject, iTween.Hash("z",0f,"time",1f,"easetype", iTween.EaseType.easeOutElastic));
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

	void OnMouseDown(){
		if (draggable) {
			screenPointDrag = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			offsetDrag = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPointDrag.z));

			// Anim to grow up
			iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (startScale + 0.2f, startScale + 0.2f, startScale + 0.2f), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));
		
			moving = true;

			// Anim empty trash
			iTween.FadeTo(trashEmptyClone, iTween.Hash("alpha", 1, "time",0.3f,"easetype", iTween.EaseType.linear));
			iTween.ScaleTo (trashEmptyClone, iTween.Hash ("scale", new Vector3(startScale, startScale, startScale), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));
		}
	}

	void OnMouseDrag(){
		if (draggable) {
			Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPointDrag.z);
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorPoint) + offsetDrag;

			transform.position = cursorPosition;
		}
	}

	void OnMouseUp(){
		if (draggable) {
			// Anim to grow down
			iTween.ScaleTo (gameObject, iTween.Hash ("scale", new Vector3 (startScale, startScale, startScale), "time", 0.2f, "easetype", iTween.EaseType.easeOutBack));

			moving = false;

			// Animate empty trash
			iTween.FadeTo(trashEmptyClone, iTween.Hash("alpha", 0f, "time",0.3f,"easetype", iTween.EaseType.linear));
			iTween.ScaleTo (trashEmptyClone, iTween.Hash ("scale", new Vector3(0.1f, 0.1f, 0.1f), "time", 0.3f, "easetype", iTween.EaseType.easeOutBack));
		}
	}

	public bool IsMoving(){
		return moving;
	}
}
