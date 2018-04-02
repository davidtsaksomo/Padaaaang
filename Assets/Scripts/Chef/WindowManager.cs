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
			middleRectTransform.localPosition = new Vector2 (0,900);
			middleRectTransform.sizeDelta = new Vector2 (800,800);
			windowRight.SetActive (false);
		} else if (windowSum == 2) {
			windowLeft.SetActive (true);
			leftRectTransform.localPosition = new Vector2 (-460f,900);
			leftRectTransform.sizeDelta = new Vector2 (700,700);
			windowMiddle.SetActive (false);
			windowRight.SetActive (true);
			rightRectTransform.localPosition = new Vector2 (460f,900);
			rightRectTransform.sizeDelta = new Vector2 (700,700);
		} else { //windowSum >= 3
			windowLeft.SetActive (true);
			leftRectTransform.localPosition = new Vector2 (-600,900);
			leftRectTransform.sizeDelta = new Vector2 (600,600);
			windowMiddle.SetActive (true);
			middleRectTransform.localPosition = new Vector2 (0,900);
			middleRectTransform.sizeDelta = new Vector2 (600,600);
			windowRight.SetActive (true);
			rightRectTransform.localPosition = new Vector2 (600,900);
			rightRectTransform.sizeDelta = new Vector2 (600,600);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
