﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckBar : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		iTween.MoveTo(gameObject,new Vector3(12.5f,-56,90),60);
	}
		
}