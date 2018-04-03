using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStatusBar : MonoBehaviour {

	public GameObject food;

	private FoodCoolDown cooldown;
	private RectTransform rectTransform;
	// Use this for initialization
	void Start () {
		cooldown = food.GetComponent<FoodCoolDown> ();
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (cooldown.maxCoolDownTime > 0) {
			float currentWidth = (cooldown.coolDownTime / cooldown.maxCoolDownTime) * 350;
			rectTransform.sizeDelta = new Vector2 (currentWidth, rectTransform.sizeDelta.y);
		} else {
			rectTransform.sizeDelta = new Vector2 (0, rectTransform.sizeDelta.y);
		}
	}
}
