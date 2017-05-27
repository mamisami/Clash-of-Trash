using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour {
    public Image imgLvl;
    public Text txtLvl;

    // Use this for initialization
    void Start () {
		
    }
	
    // Update is called once per frame
    void Update () {
		
    }

	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void LoadLevel(bool isSinglePlayer)
	{
		Global.isSinglePlayer = isSinglePlayer;
		SceneManager.LoadScene("Level");
	}

	public void LoadGame()
	{
		SceneManager.LoadScene("StartScene");
	}

    public void ChangeLevel(int numLevel)
    {
		Global.level = numLevel;

        //imgLvl.sprite = Resources.Load<Sprite>("Sprites/Background/level"+numLevel);
        //txtLvl.text = "Level " + numLevel;

		LoadGame();
    }

	public void playTutorial() {
		StartCoroutine(playTutorialRoutine());
	}

	public IEnumerator playTutorialRoutine() {
		Handheld.PlayFullScreenMovie("tuto.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
		yield return new WaitForEndOfFrame();
	}
}
