﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		scale = transform.localScale;
	}


	Text text;

	public float timeLeft = /*60*2*/6;
	public Color blink1 = Color.red;
	public Color blink2 = Color.yellow;

	Vector2 scale;
	void Update()
	{
		float oldTimeLeft = timeLeft;
		timeLeft -= Time.deltaTime;
		if (check (0, oldTimeLeft)) {
			TimeFinish ();
		} else if (check (1, oldTimeLeft)) {
			setColor (blink2);
			growDown();
		} else if (check (2, oldTimeLeft)) {
			setColor (blink1);
			growDown ();
		}
		else if (check (3, oldTimeLeft)) {
			setColor (blink2);
			growDown ();
		}else if (check (4, oldTimeLeft)) {
			setColor (blink1);
			growDown();
		}else if (check (5, oldTimeLeft)) {
			setColor (blink2);
			growDown ();
		}
		else {
			// GrowDown all 10 seconds
			for(int i = 10; i<oldTimeLeft;i+=10){
				if (check (i + 1, oldTimeLeft)) {
					growDown ();
				}					
			}
		}

		// Rotate
		for(int i = 1; i<oldTimeLeft;i++){
			if (check (i + 1, oldTimeLeft)) {
				float z = 5;
				iTween.RotateTo(gameObject, iTween.Hash("z",(i % 2 == 0) ? z : -z,"time",1f,"easetype", iTween.EaseType.easeOutQuart));
				break;
			}
		}
	}

	bool check(float sec, float oldTimeLeft){
		return timeLeft <= sec && oldTimeLeft > sec;
	}

	void setColor(Color c){
		text.color = c;
	}

	void growDown(){
		//iTween.ScaleTo(gameObject, iTween.Hash("scale",scale,"time",0.8f,"easetype", iTween.EaseType.easeOutBack));

		iTween.ValueTo(gameObject, iTween.Hash(
			"from", scale + scale*0.2f,
			"to", scale,
			"time", 0.8f,
			"onupdatetarget", this.gameObject, 
			"easetype", iTween.EaseType.easeOutBack,
			"onupdate", "OnRescale"));
	}
		
	void OnRescale(Vector2 scale){
		transform.localScale = scale;
	}

	void TimeFinish(){
		// TODO: STOP all
	}

	void OnGUI(){
		if (timeLeft >= 0) {
			string minutes = Mathf.Floor (timeLeft / 60).ToString ("00");
			string seconds = Mathf.Floor (timeLeft % 60).ToString ("00");
			text.text = minutes + ":" + seconds;
		}
	}
}