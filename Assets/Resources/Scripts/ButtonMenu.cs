using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour {
    public Image imgLvl;
    public Text txtLvl;

	private int selectedLevel = 1;

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
		Global.level = selectedLevel;
		SceneManager.LoadScene("StartScene");
	}

    public void ChangeLevel(int numLevel)
    {
		selectedLevel = numLevel;

        imgLvl.sprite = Resources.Load<Sprite>("Sprites/Background/level"+numLevel);
        txtLvl.text = "Level " + numLevel;
    }
}
