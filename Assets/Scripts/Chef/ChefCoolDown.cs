using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefCoolDown : MonoBehaviour {
	public bool isInUse;
	private GameObject cookedFood;


	// Use this for initialization
	void Start () {
		isInUse = false;
		cookedFood = null;
		InvokeRepeating ("CheckInUse", 0f, 0.1f);

	}

	public void CookThis(GameObject food){
		cookedFood = food;
		isInUse = true;
		Image image = GetComponent<Image> ();
		image.enabled = false;
	}
	
	// Update is called once per frame
	void CheckInUse () {
		if (isInUse) {
			FoodCoolDown coolDown = cookedFood.GetComponent<FoodCoolDown> ();
			if (!coolDown.isInCoolDown) {
				isInUse = false;
				cookedFood = null;
				Image image = GetComponent<Image> ();
				image.enabled = true;
			}
		}
	}
}
