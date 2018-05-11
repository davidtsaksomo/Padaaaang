using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenScript : MonoBehaviour {
	public float delay;
	public float fadeDelay;
	public RectTransform logo;
	public Image blank;
	public float scale;
	private float curTime;

	// Use this for initialization
	void Start () {
		StartCoroutine (LoadTitleAfterDelay (delay));
		curTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		logo.sizeDelta += new Vector2(logo.sizeDelta.x * scale * Time.deltaTime,
			logo.sizeDelta.y * scale * Time.deltaTime);
		if (curTime > delay - fadeDelay) {
			Color temp = blank.color;
			temp.a += Time.deltaTime / fadeDelay;
			blank.color = temp;
		}
		curTime += Time.deltaTime;
	}

	IEnumerator LoadTitleAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Application.LoadLevel("Title");
	}
}
