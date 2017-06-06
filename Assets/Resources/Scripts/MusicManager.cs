using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage all the background music
/// </summary>
public class MusicManager : MonoBehaviour {

	static AudioSource audioSource;

	public static MusicManager instance;
	private static string playedMusic = "";


	/// <summary>
	/// Changes the music.
	/// </summary>
	/// <param name="musicResource">Music resource.</param>
	public static void changeMusic(string musicResource) {
		playedMusic = musicResource;
		audioSource.Stop();
		audioSource.pitch = 1.0f; 	// Default pitch
		audioSource.volume = 1.0f;	// Default volume
		audioSource.clip = Resources.Load<AudioClip> (musicResource);
		audioSource.Play();
	}

	/// <summary>
	/// Speeds up the music.
	/// </summary>
	/// <param name="acceleration">Acceleration.</param>
	public static void SpeedUp(float acceleration) {
		audioSource.pitch += acceleration;
	}


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

		string newMusic = "";
		if (newScene.name == "StartScene") {
			switch (Global.level) {
			case 1:
				newMusic = "Music/Level1";
				break;
			case 2:
				newMusic = "Music/Level2";
				break;
			}
			changeMusic(newMusic);

		} else if (newScene.name == "Menu" && playedMusic != "Music/Menu") {
			newMusic = "Music/Menu";
			changeMusic(newMusic);
		}
	}
}
	