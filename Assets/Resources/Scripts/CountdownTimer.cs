using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Time of the game controller
/// </summary>
public class CountdownTimer : MonoBehaviour {

	GameObject finishMenu;
	Text text;

	private float timeLeft = Global.GAME_TIME;

	Vector2 scale;

	void Start () {
		text = GetComponent<Text> ();
		scale = transform.localScale;
		finishMenu = GameObject.Find ("/Canvas/Finish");		
		finishMenu.SetActive (false);
	}

	void Update()
	{
		text.enabled = Global.isStart;

		//If the game is started, take the time
		if (Global.isStart) {
			float oldTimeLeft = timeLeft;
			timeLeft -= Time.deltaTime;

			//Check the time for make it blink
			if (check (0, oldTimeLeft)) {
				TimeFinish ();
			} else if (check (1, oldTimeLeft)) {
				setColor (Global.TIMER_BLINK_COLOR_1);
				growDown();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (2, oldTimeLeft)) {
				setColor (Global.TIMER_BLINK_COLOR_2);
				growDown();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (3, oldTimeLeft)) {
				setColor (Global.TIMER_BLINK_COLOR_1);
				growDown ();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (4, oldTimeLeft)) {
				setColor (Global.TIMER_BLINK_COLOR_2);
				growDown ();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (5, oldTimeLeft)) {
				setColor (Global.TIMER_BLINK_COLOR_1);
				growDown();
				MakeSound ("Music/Alarm", 1.0f);
			} else if (check (6, oldTimeLeft)) {
				setColor (Global.TIMER_BLINK_COLOR_2);
				growDown ();
				MakeSound ("Music/Alarm", 1.0f);
			}
			else {
				// GrowDown all 10 seconds
				for(int i = 10; i<oldTimeLeft;i+=10){
					if (check (i + 1, oldTimeLeft)) {
						growDown ();

						//Accelerate the music
						MusicManager.SpeedUp (0.01f);
					}					
				}
			}

			//At ten second before the end, make a huge acceleration of the music
			if(check(11, oldTimeLeft))
				MusicManager.SpeedUp (0.5f);
			
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

	/// <summary>
	/// Check the time
	/// </summary>
	/// <param name="sec">Searched time</param>
	/// <param name="oldTimeLeft">old time</param>
	/// <returns></returns>
	bool check(float sec, float oldTimeLeft){
		return timeLeft <= sec && oldTimeLeft > sec;
	}

	/// <summary>
	/// Set the text color
	/// </summary>
	/// <param name="c">Color to set</param>
	void setColor(Color c){
		text.color = c;
	}

	/// <summary>
	/// Make the last five second sound
	/// </summary>
	/// <param name="resource">Ressource of the sound</param>
	/// <param name="volume">Volume</param>
	/// <param name="time">Period of time</param>
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

	/// <summary>
	/// Grow down the text
	/// </summary>
	void growDown(){
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

	/// <summary>
	/// Action to do when the time is over
	/// </summary>
	void TimeFinish(){
		Global.isStart = false;

		//Hide all draggable
		GameObject spawn = GameObject.FindGameObjectWithTag ("SpawnManager");
		if (spawn)
			spawn.GetComponent<SpawnManager> ().HideAll ();

		//Destroy the truckbar
		GameObject truckBar = GameObject.FindWithTag ("TruckBar");
		if (truckBar)
			Destroy (truckBar);

		GameObject btnQuit = GameObject.Find ("BtnQuit");
		if (btnQuit)
			Destroy (btnQuit);

		//Show the finish menu
		Vector3 pos = finishMenu.transform.position;
		finishMenu.SetActive (true);
		float valStart = 50f;
		pos.y += valStart;
		finishMenu.transform.position = pos;
		iTween.MoveTo (finishMenu, iTween.Hash ("position", new Vector3 (pos.x, pos.y-valStart, pos.z), "time", 1f, "easetype", iTween.EaseType.easeOutBounce));
	
		PlayerController player;
		GameObject localPlayerObject = GameObject.FindGameObjectWithTag ("LocalPlayer");
		if (localPlayerObject != null) {
			player = localPlayerObject.GetComponent<PlayerController> ();
			player.generateExplanationScrollView ();
		}
	
	}

	void OnGUI(){
		//Display the time
		if (timeLeft >= 0) {
			string minutes = Mathf.Floor (timeLeft / 60).ToString ("00");
			string seconds = Mathf.Floor (timeLeft % 60).ToString ("00");
			text.text = minutes + ":" + seconds;
		}
	}
}
