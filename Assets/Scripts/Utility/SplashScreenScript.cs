using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenScript : MonoBehaviour {
	public float delay;
	public RectTransform logo;
	public float scale;

	// Use this for initialization
	void Start () {
		StartCoroutine (LoadTitleAfterDelay (delay));
	}
	
	// Update is called once per frame
	void Update () {
		logo.sizeDelta += new Vector2(logo.sizeDelta.x * scale * Time.deltaTime,
			logo.sizeDelta.y * scale * Time.deltaTime);
	}

	IEnumerator LoadTitleAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Application.LoadLevel("Title");
	}
}
