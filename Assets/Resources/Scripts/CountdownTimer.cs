using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {

	GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		scale = transform.localScale;
		soundManager = GameObject.Find ("Music").GetComponent<SoundManager> ();
		pauseMenu = GameObject.Find ("/Canvas/Pause");		
		//pauseMenu.transform.position.y += 200f;
		pauseMenu.SetActive (false);
	}

	SoundManager soundManager;
	Text text;

	private float timeLeft = Global.GAME_TIME;
	public Color blink1 = Color.red;
	public Color blink2 = Color.yellow;

	Vector2 scale;
	void Update()
	{
		text.enabled = Global.isStart;

		if (Global.isStart) {
			float oldTimeLeft = timeLeft;
			timeLeft -= Time.deltaTime;
			if (check (0, oldTimeLeft)) {
				TimeFinish ();
			} else if (check (1, oldTimeLeft)) {
				setColor (blink1);
				growDown();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (2, oldTimeLeft)) {
				setColor (blink2);
				growDown();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (3, oldTimeLeft)) {
				setColor (blink1);
				growDown ();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (4, oldTimeLeft)) {
				setColor (blink2);
				growDown ();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (5, oldTimeLeft)) {
				setColor (blink1);
				growDown();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (6, oldTimeLeft)) {
				setColor (blink2);
				growDown ();
				MakeSound ("Music/Alarm", 1.0f);
			}
			else {
				// GrowDown all 10 seconds
				for(int i = 10; i<oldTimeLeft;i+=10){
					if (check (i + 1, oldTimeLeft)) {
						growDown ();
						soundManager.SpeedUp (0.01f);
					}					
				}
			}

			if(check(11, oldTimeLeft))
				soundManager.SpeedUp (0.5f);
			
			// Rotate
			for(int i = 1; i<oldTimeLeft;i++){
				if (check (i + 1, oldTimeLeft)) {
					float z = 5;
					iTween.RotateTo(gameObject, iTween.Hash("z",(i % 2 == 0) ? z : -z,"time",1f,"easetype", iTween.EaseType.easeOutQuart));
					break;
				}
			}
		}
	}

	bool check(float sec, float oldTimeLeft){
		return timeLeft <= sec && oldTimeLeft > sec;
	}

	void setColor(Color c){
		text.color = c;
	}

	public void MakeSound(string resource, float volume, float time=0.0f){
		//play sound and destroy audio source
		AudioClip myClip = Resources.Load<AudioClip>(resource);
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = myClip;
		audioSource.time = time;
		audioSource.volume = volume;
		audioSource.Play();
		Destroy(audioSource, myClip.length);
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
		Global.isStart = false;

		GameObject spawn = GameObject.FindGameObjectWithTag ("SpawnManager");
		spawn.GetComponent<SpawnManager> ().HideAll ();

		//Show menu
		Vector3 pos = pauseMenu.transform.position;
		pauseMenu.SetActive (true);
		float valStart = 50f;
		pos.y += valStart;
		pauseMenu.transform.position = pos;
		iTween.MoveTo (pauseMenu, iTween.Hash ("position", new Vector3 (pos.x, pos.y-valStart, pos.z), "time", 1f, "easetype", iTween.EaseType.easeOutBounce));
	
		PlayerController player;
		GameObject localPlayerObject = GameObject.FindGameObjectWithTag ("LocalPlayer");
		if (localPlayerObject != null) {
			player = localPlayerObject.GetComponent<PlayerController> ();
			player.generateExplanationScrollView ();
		}
	
	}

	void OnGUI(){
		if (timeLeft >= 0) {
			string minutes = Mathf.Floor (timeLeft / 60).ToString ("00");
			string seconds = Mathf.Floor (timeLeft % 60).ToString ("00");
			text.text = minutes + ":" + seconds;
		}
	}
}
