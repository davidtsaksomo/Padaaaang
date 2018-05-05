using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefCoolDown : MonoBehaviour {
	public bool isInUse;
	private GameObject cookedFood;
	private AudioSource fryingSound;


	// Use this for initialization
	void Start () {
		isInUse = false;
		cookedFood = null;
		InvokeRepeating ("CheckInUse", 0f, 0.1f);
		fryingSound = GetComponent<AudioSource> ();
	}

	public void CookThis(GameObject food){
		cookedFood = food;
		isInUse = true;
		Image image = GetComponent<Image> ();
		image.enabled = false;
		if (fryingSound != null) {
			fryingSound.time = 0f;
			fryingSound.Play ();
		}
	}
	
	// Update is called once per frame
	void CheckInUse () {
		if (fryingSound.time > 11f) {
			fryingSound.time = 3f;
		}
		if (isInUse) {
			FoodCoolDown coolDown = cookedFood.GetComponent<FoodCoolDown> ();
			if (!coolDown.isInCoolDown) {
				isInUse = false;
				cookedFood = null;
				Image image = GetComponent<Image> ();
				image.enabled = true;
				fryingSound.Stop ();
			}
		}
	}
}
