using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
	public NetworkDiscovery discovery;
	public CoT_NetworkManager manager;

	public Image imgLvl;

	public Text txtTimer;

	float timerMultiplayer = 5.0f;
	bool timerEnabled = !Global.isSinglePlayer;

	GameObject quitMenu;

	// Use this for initialization
	void Start () {
		imgLvl.sprite = Resources.Load<Sprite>("Sprites/Background/level"+Global.level);

		//if (Global.level == 2)
		txtTimer.color = Color.grey;

		if (Global.isSinglePlayer) {
			NetworkManager.singleton.StartHost ();
		} else {
			discovery.broadcastKey = discovery.broadcastKey + Global.level;
			discovery.Initialize();
			discovery.StartAsClient();
		}

		// Set listener quit button
		Button btn = GameObject.Find("CanvasInfos/BtnQuit").GetComponent<Button>();
		btn.onClick.AddListener(ShowQuitPanel);

		// Set listener continue button
		Button btnContinue = GameObject.Find("BtnContinue").GetComponent<Button>();
		btnContinue.onClick.AddListener(HideQuitPanel);

		// Remove replay if multi
		Debug.Log(Global.isSinglePlayer);
		GameObject btnReplay = GameObject.Find("Quit/Panel/BtnReplay");
		Debug.Log (btnReplay);
		btnReplay.gameObject.SetActive (Global.isSinglePlayer);

		// Disable quit menu
		quitMenu = GameObject.Find ("/Canvas/Quit");		
		quitMenu.SetActive (false);
	}


	void ShowQuitPanel(){
		Vector3 pos = quitMenu.transform.position;
		quitMenu.SetActive (true);
		float valStart = 10f;
		pos.y += valStart;
		quitMenu.transform.position = pos;
		iTween.MoveTo (quitMenu, iTween.Hash ("position", new Vector3 (pos.x, pos.y-valStart, pos.z), "time", 0.5f, "easetype", iTween.EaseType.easeOutBounce));
	}

	void HideQuitPanel(){
		quitMenu.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (timerEnabled) {
			timerMultiplayer -= Time.deltaTime;
			if (timerMultiplayer < 0) {
				timerEnabled = false;

				if (!manager.isNetworkActive) {
					discovery.StopBroadcast ();
					manager.isServer = true;
					manager.StartHost ();
				}
			}
		}
	}
}
