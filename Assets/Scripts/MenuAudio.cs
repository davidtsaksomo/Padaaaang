using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour {
	private AudioSource sounds;
	public AudioClip shakingGlass;
	public AudioClip slidingSpoon;
	public AudioClip slidingSpoonReverse;
	public AudioClip tappingChopsticks;
	public AudioClip tappingSpoon;
	public AudioClip kucingDatang;
	public AudioClip kucingPergi;
	public AudioClip satpam;
	public AudioClip telefon;
	public AudioClip terimaMakanan;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void shakeGlass(){
		playClip (shakingGlass);
	}

	public void slideSpoon(){
		playClip (slidingSpoon);
	}

	public void slideSpoonReverse(){
		playClip (slidingSpoonReverse);
	}

	public void tapChopsticks(){
		playClip (tappingChopsticks);
	}

	public void tapSpoon(){
		playClip (tappingSpoon);
	}

	public void playKucingMasuk(){
		playClip (kucingDatang);
	}

	public void playKucingPergi(){
		playClip (kucingPergi);
	}

	public void playSatpam(){
		playClip (satpam);
	}

	public void playTerimaMakanan(){
		playClip (terimaMakanan);
	}

	void playClip(AudioClip clip){
		sounds.time = 0f;
		sounds.clip = clip;
		sounds.Play ();
	}
}
