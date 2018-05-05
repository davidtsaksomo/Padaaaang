using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

	float widthMax;
	void Start(){
		widthMax = GetComponent<RectTransform> ().sizeDelta.x;
	}
	public void setValue(float value){
		Color color = new Color();
		color.r = 1f - Mathf.Max((value-0.5f),0f)*2;
		color.g = Mathf.Min((value),0.5f)*2;
		color.a = 1f;
		color.b = 0;
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (widthMax * value, GetComponent<RectTransform> ().sizeDelta.y);
		GetComponent<Image> ().color = color;
	}
}
