﻿using UnityEngine;

public class SoundManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
		string music = "";
        switch (Global.level)
        {
            case 1:
                music = "Music/Music1";
                break;
            case 2:
			 	music = "Music/Music2";
                break;
        }
		audioSource.clip = Resources.Load<AudioClip>(music);
		audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
