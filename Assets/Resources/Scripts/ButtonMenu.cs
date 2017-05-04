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

    public void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }

    public void ChangeLevel(int numLevel)
    {
        imgLvl.sprite = Resources.Load<Sprite>("Sprites/Background/level"+numLevel);
        txtLvl.text = "Level " + numLevel;
    }
}
