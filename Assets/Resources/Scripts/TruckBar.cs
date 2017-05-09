using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckBar : MonoBehaviour {

	float positionX = 0;
	// Use this for initialization
	void Start () {
		positionX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		//iTween.MoveTo(gameObject,new Vector3(12.5f,-56,90),60);
		iTween.MoveTo(gameObject,  iTween.Hash("x", positionX+1.6f,"time",2,"easetype", iTween.EaseType.linear/*,"onComplete","SideLeft"*/));
	}
		
}
