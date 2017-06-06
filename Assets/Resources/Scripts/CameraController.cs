using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
	public NetworkDiscovery discovery;
	public CoT_NetworkManager manager;

	/// <summary>
	/// Background image
	/// </summary>
	public Image imgLvl;

	public Text txtTimer;

	/// <summary>
	/// Timer of search multiplayer game before become a server
	/// </summary>
	float timerMultiplayer = 5.0f;
	/// <summary>
	/// Enable the multiplayer connection timer
	/// </summary>
	bool timeMultiplayerEnabled = !Global.isSinglePlayer;

	GameObject quitMenu;

	/// <summary>
	/// Load background image, Start network things and load quit panel
	/// </summary>
	void Start () {
		imgLvl.sprite = Resources.Load<Sprite>("Sprites/Background/level"+Global.level);

		//if (Global.level == 2)
		txtTimer.color = Color.grey;

		//If we are in multi, start the discovery of a server
		if (Global.isSinglePlayer) {
			NetworkManager.singleton.StartHost ();
		} else {
			discovery.broadcastKey = discovery.broadcastKey + Global.level; //Search a multiplayer server with the same level
			discovery.Initialize();
			discovery.StartAsClient();
		}

		// Set listener quit button
		Button btn = GameObject.Find("CanvasTimeScore/BtnQuit").GetComponent<Button>();
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

	/// <summary>
	/// Show the quit panel
	/// </summary>
	void ShowQuitPanel(){
		Vector3 pos = quitMenu.transform.position;
		quitMenu.SetActive (true);
		float valStart = 10f;
		pos.y += valStart;
		quitMenu.transform.position = pos;
		iTween.MoveTo (quitMenu, iTween.Hash ("position", new Vector3 (pos.x, pos.y-valStart, pos.z), "time", 0.5f, "easetype", iTween.EaseType.easeOutBounce));
	}

	/// <summary>
	/// Hide the quit panel
	/// </summary>
	void HideQuitPanel(){
		quitMenu.SetActive (false);
	}

	/// <summary>
	/// Update the multiplayer timer
	/// </summary>
	void Update () {
		if (timeMultiplayerEnabled) {
			timerMultiplayer -= Time.deltaTime;
			if (timerMultiplayer < 0) {
				//If in the given time we don't have find a server, we become the server
				timeMultiplayerEnabled = false;

				if (!manager.isNetworkActive) {
					discovery.StopBroadcast ();
					manager.isServer = true;
					manager.StartHost ();
				}
			}
		}
	}
}
