using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

	static AudioSource audioSource;

	public static MusicManager instance;
	private static string music = "";

	void Awake() {
		if (instance == null) {
			instance = this;
			SceneManager.activeSceneChanged += OnSceneChange;
			DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (this.gameObject);
		}
	}
		
	void OnSceneChange(Scene previousScene, Scene newScene) {
		// Note: previousScene is always empty because of the singleton structure.
		audioSource = GetComponent<AudioSource>();

		if (newScene.name == "StartScene") {
			switch (Global.level) {
			case 1:
				music = "Music/Music1";
				break;
			case 2:
				music = "Music/Music2";
				break;
			}
			changeMusic();

		} else if (newScene.name == "Menu" && music != "Music/MenuMusic") {
			music = "Music/MenuMusic";
			changeMusic();
		}
	}
	static void changeMusic(){
		audioSource.Stop();
		audioSource.pitch = 1.0f; 	// Default pitch
		audioSource.volume = 1.0f;	// Default volume
		audioSource.clip = Resources.Load<AudioClip> (music);
		audioSource.Play();
	}

	public static void SpeedUp(float acceleration){
		audioSource.pitch += acceleration;
	}
}
	