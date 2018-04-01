using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodCoolDown : MonoBehaviour {
	public float coolDownTime;
	public float binCoolDown;
	public float emptyCoolDown;
	public float cookCoolDown;
	public bool isInCoolDown;

	// Use this for initialization
	void Start () {
		coolDownTime = 0;
		InvokeRepeating ("decCoolDown", 0f, 0.1f);
		isInCoolDown = false;
	}

	void setCoolDownTime(float time){
		if (time > coolDownTime) {
			coolDownTime = time;
			isInCoolDown = true;
		}
	}

	void decCoolDown(){
		if (coolDownTime > 0) {
			coolDownTime -= 0.1f;
		}
		if (coolDownTime < 0) {
			coolDownTime = 0;
		}
		if (coolDownTime > 0) {
			//COOLDOWN IMAGE HERE
			Image image = GetComponent<Image> ();
			image.enabled = false;
		} else {
			//UNCOOLDOWN IMAGE HERE
			Image image = GetComponent<Image> ();
			image.enabled = true;
		}
		isInCoolDown = coolDownTime > 0;
	}
	
	public void emptyFood(){
		setCoolDownTime (emptyCoolDown);
	}

	public void binFood(){
		setCoolDownTime (binCoolDown);
	}

	public void cookFood(){
		setCoolDownTime (cookCoolDown);
	}
}
