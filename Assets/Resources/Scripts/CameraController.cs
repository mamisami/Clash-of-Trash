using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour {
	public NetworkDiscovery discovery;
	public NetworkManager manager;

	float timerMultiplayer = 5.0f;
	bool timerEnabled = !Constant.IS_SINGLE_PLAYER;

	// Use this for initialization
	void Start () {
		if (!Constant.IS_SINGLE_PLAYER) {
			discovery.Initialize();
			discovery.StartAsClient();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (timerEnabled) {
			timerMultiplayer -= Time.deltaTime;
			if (timerMultiplayer < 0) {
				timerEnabled = false;

				if (!manager.isNetworkActive) {
					discovery.StopBroadcast ();
					manager.StartHost ();
				}
			}
		}
	}
}
