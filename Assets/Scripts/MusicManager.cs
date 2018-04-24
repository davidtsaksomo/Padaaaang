﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	public AudioClip themeMusic;
	public AudioClip inGameMusic;
	public float fadeSpeed;
	public string playingMusic = "";
	public float volume;
	private AudioSource sounds;
	private static bool created = false;

	void Awake(){
		if (!created) {
			DontDestroyOnLoad (this.gameObject);
			created = true;
		}
	}

	// Use this for initialization
	void Start () {
		sounds = GetComponent<AudioSource> ();
		playTheme ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playTheme(){
		sounds.clip = themeMusic;
		sounds.time = 0f;
		sounds.Play();
		InvokeRepeating ("loopTheme",0f,0.02f);
		playingMusic = "theme";
	}

	public void loopTheme(){
		if(sounds.time >= 72.32f){
			sounds.time = 6.57f;
		}
	}

	public void stopTheme(){
		CancelInvoke ("loopTheme");
		Debug.Log ("fade");
		InvokeRepeating ("fadeOut",0f,0.1f);
		playingMusic = "";
	}

	public void setVolume(float vol){
		sounds.volume = vol;
	}
	public void fadeOut(){
		sounds.volume = sounds.volume * fadeSpeed;
		if(sounds.volume <= 0.01){
			sounds.volume = 0;
			sounds.Stop();
			CancelInvoke("fadeOut");
		}
	}
}
