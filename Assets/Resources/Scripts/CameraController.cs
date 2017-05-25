using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
	public NetworkDiscovery discovery;
	public NetworkManager manager;

	public Image imgLvl;

	public Text txtTimer;

	float timerMultiplayer = 5.0f;
	bool timerEnabled = !Global.isSinglePlayer;

	// Use this for initialization
	void Start () {
		imgLvl.sprite = Resources.Load<Sprite>("Sprites/Background/level"+Global.level);

		if (Global.level == 2)
			txtTimer.color = Color.grey;

		if (Global.isSinglePlayer) {
			NetworkManager.singleton.StartHost ();
		} else {
			discovery.broadcastKey = discovery.broadcastKey + Global.level;
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
