using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controller for buttons of the menu
/// </summary>
public class ButtonMenu : MonoBehaviour {

    /// <summary>
    /// Control if the user want to restart a game
    /// </summary>
    void Start () {
		//Test if the user want to restart the game
		if (Global.reloadGame == true) {
			Global.reloadGame = false;

			//Restart the game
			LoadGame();
		}
    }

	/// <summary>
	/// Load the menu scene
	/// </summary>
	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	/// <summary>
	/// Load the game world selection scene
	/// </summary>
	/// <param name="isSinglePlayer">Define if the user want to be in single player mode</param>
	public void LoadLevel(bool isSinglePlayer)
	{
		Global.isSinglePlayer = isSinglePlayer;
		SceneManager.LoadScene("Level");
	}

	/// <summary>
	/// Load game scene
	/// </summary>
	public void LoadGame()
	{
		SceneManager.LoadScene("StartScene");
	}

	/// <summary>
	/// Load a specifiq level and start the game
	/// </summary>
	/// <param name="numLevel">Level to load</param>
    public void ChangeLevel(int numLevel)
    {
		Global.level = numLevel;

		LoadGame();
    }

	/// <summary>
	/// End the game (go to menu or replay)
	/// </summary>
	/// <param name="replay">Boolean which indicate if the user want to restart the game</param>
	public void EndGame(bool replay) {
		Global.reloadGame = replay;

		//Stop all network things (for liberate all ports)
		if (NetworkManager.singleton.isNetworkActive) {
			NetworkManager.singleton.StopServer();
			NetworkManager.singleton.StopClient();
		}
	}

	/// <summary>
	/// Start the tutorial video
	/// </summary>
	public void playTutorial() {
		StartCoroutine(playTutorialRoutine());
	}

	/// <summary>
	/// Create co routine with the tutorial video
	/// </summary>
	/// <returns>co routine</returns>
	public IEnumerator playTutorialRoutine() {
		Handheld.PlayFullScreenMovie("tuto.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
		yield return new WaitForEndOfFrame();
	}
}
