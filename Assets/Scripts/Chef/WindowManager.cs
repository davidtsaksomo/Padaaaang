using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {
	public GameObject windowLeft;
	public GameObject windowMiddle;
	public GameObject windowRight;
	public int windowSum;
	// Use this for initialization
	void Start () {
		RectTransform leftRectTransform = windowLeft.GetComponent<RectTransform>();
		RectTransform middleRectTransform = windowMiddle.GetComponent<RectTransform>();
		RectTransform rightRectTransform = windowRight.GetComponent<RectTransform>();
		if (windowSum <= 1) {
			windowLeft.SetActive (false);
			windowMiddle.SetActive (true);
			middleRectTransform.localPosition = new Vector2 (0,772);
			middleRectTransform.sizeDelta = new Vector2 (785,667);
			windowRight.SetActive (false);
		} else if (windowSum == 2) {
			windowLeft.SetActive (true);
			leftRectTransform.localPosition = new Vector2 (-460f,772f);
			leftRectTransform.sizeDelta = new Vector2 (610.5f,518.5f);
			windowMiddle.SetActive (false);
			windowRight.SetActive (true);
			rightRectTransform.localPosition = new Vector2 (460f,772f);
			rightRectTransform.sizeDelta = new Vector2 (610.5f,518.5f);
		} else { //windowSum >= 3
			windowLeft.SetActive (true);
			leftRectTransform.localPosition = new Vector2 (-575,772);
			leftRectTransform.sizeDelta = new Vector2 (436,370);
			windowMiddle.SetActive (true);
			middleRectTransform.localPosition = new Vector2 (0,772);
			middleRectTransform.sizeDelta = new Vector2 (436,370);
			windowRight.SetActive (true);
			rightRectTransform.localPosition = new Vector2 (575,772);
			rightRectTransform.sizeDelta = new Vector2 (436,370);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
