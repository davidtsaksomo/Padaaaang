using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {
	public GameObject[] window;
	public int windowSum;
	// Use this for initialization
	void Start () {
		RectTransform leftRectTransform = window[0].GetComponent<RectTransform>();
		RectTransform middleRectTransform = window[1].GetComponent<RectTransform>();
		RectTransform rightRectTransform = window[2].GetComponent<RectTransform>();
		if (windowSum <= 1) {
			window[0].SetActive (true);
			window[1].SetActive (false);
			leftRectTransform.localPosition = new Vector2 (0,900);
			leftRectTransform.sizeDelta = new Vector2 (800,800);
			window[2].SetActive (false);
		} else if (windowSum == 2) {
			window[0].SetActive (true);
			leftRectTransform.localPosition = new Vector2 (-460f,900);
			leftRectTransform.sizeDelta = new Vector2 (700,700);
			window[1].SetActive (true);
			window[2].SetActive (false);
			middleRectTransform.localPosition = new Vector2 (460f,900);
			middleRectTransform.sizeDelta = new Vector2 (700,700);
		} else { //windowSum >= 3
			window[0].SetActive (true);
			leftRectTransform.localPosition = new Vector2 (-600,900);
			leftRectTransform.sizeDelta = new Vector2 (600,600);
			window[1].SetActive (true);
			middleRectTransform.localPosition = new Vector2 (0,900);
			middleRectTransform.sizeDelta = new Vector2 (600,600);
			window[2].SetActive (true);
			rightRectTransform.localPosition = new Vector2 (600,900);
			rightRectTransform.sizeDelta = new Vector2 (600,600);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
