using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMusic : MonoBehaviour {
	public AudioSource music1;
	public AudioSource music2;
	public AudioSource music3;
	public AudioSource music4;
	private float music1vol=1;
	private float music2vol=0;
	private float music3vol=0;
	private float music4vol=0;
	private LifeBar lifebar;
	private static bool created = false;

	void Awake(){
		if (!created) {
			DontDestroyOnLoad (this.gameObject);
			created = true;
		}
	}

	public void init(){
		lifebar = GameObject.Find ("Life").GetComponent<LifeBar> ();
		music1vol = 1f;
		music2vol = 0f;
		music3vol = 0f;
		music4vol = 0f;
	}

	void Update(){
		if (lifebar != null) {
			float percentage = lifebar.percentage;
			if (percentage > 0.75f) {
				music2vol = 0;
				music3vol = 0;
				music4vol = 0;
			} else if (percentage <= 0.75f && percentage > 0.5f) {
				music2vol = 0;
				music3vol = 1 - ((percentage - 0.5f) * 4);
				music4vol = 0;
			} else if (percentage <= 0.5f && percentage > 0.25f) {
				music2vol = 1 - (percentage - 0.25f) * 4;
				music3vol = 1 - music2vol;
				music4vol = 0;
			} else if (percentage <= 0.25f) {
				music2vol = 1;
				music3vol = 0;
				music4vol = 1 - (percentage * 4);
			}
		}
	}

	public void startMusic(){
		music1.Play ();
		music2.Play ();
		music3.Play ();
		music4.Play ();
		InvokeRepeating ("loopMusic", 0f, 0.02f);
	}

	void loopMusic(){
		if (music1.time >= 26.12f) {
			music1.time = 0;
			music2.time = 0;
			music3.time = 0;
			music4.time = 0;
		}
		float volChangeRate = 0.02f;
		music1.volume += (music1vol - music1.volume) * volChangeRate;
		music2.volume += (music2vol - music2.volume) * volChangeRate;
		music3.volume += (music3vol - music3.volume) * volChangeRate;
		music4.volume += (music4vol - music4.volume) * volChangeRate;
	}

	public void stopMusic(){
		music1.Stop ();
		music2.Stop ();
		music3.Stop ();
		music4.Stop ();
		CancelInvoke ("loopMusic");
	}
}
