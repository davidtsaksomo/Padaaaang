using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCoolDown : MonoBehaviour {
	public float coolDownTime;
	public float maxCoolDownTime;
	public float binCoolDown;
	public float emptyCoolDown;
	public float cookCoolDown;
	public bool isInCoolDown;
	public Sprite emptySprite;
	Sprite foodSprite;
	// Use this for initialization
	void Start () {
		coolDownTime = 0;
		maxCoolDownTime = 0;
		InvokeRepeating ("decCoolDown", 0f, 0.1f);
		isInCoolDown = false;
		foodSprite = GetComponent<Image>().sprite;
	}

	void setCoolDownTime(float time){
		coolDownTime = time;
		if (time > maxCoolDownTime) {
			maxCoolDownTime = time;
		}
	}

	void decCoolDown(){
		if (coolDownTime > 0) {
			coolDownTime -= 0.1f;
		}
		if (coolDownTime < 0) {
			coolDownTime = 0;
		}
		if (coolDownTime == 0) {
			maxCoolDownTime = 0;
		}
		checkCoolDown ();
	}

	void checkCoolDown(){
		if (coolDownTime > 0) {
			//COOLDOWN IMAGE HERE
			Image image = GetComponent<Image> ();
			image.sprite = emptySprite;
		} else {
			//UNCOOLDOWN IMAGE HERE
			Image image = GetComponent<Image> ();
			image.sprite = foodSprite;
		}
		isInCoolDown = coolDownTime > 0;
	}

	public void emptyFood(){
		setCoolDownTime (emptyCoolDown);
		checkCoolDown ();
	}

	public void binFood(){
		setCoolDownTime (binCoolDown);
		checkCoolDown ();

	}

	public void cookFood(){
		setCoolDownTime (cookCoolDown);
		checkCoolDown ();

	}
}
