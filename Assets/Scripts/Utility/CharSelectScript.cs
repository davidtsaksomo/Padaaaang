using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectScript : MonoBehaviour {

	[SerializeField] GameObject choose;
	[SerializeField] GameObject bg_all_blur;
	[SerializeField] GameObject bg_no_blur;
	[SerializeField] GameObject bg_girl;
	[SerializeField] GameObject bg_boy;
	[SerializeField] GameObject box;
	[SerializeField] GameObject button_girl;
	[SerializeField] GameObject button_boy;
	public float stopTimer;
	public float stopTimer2;
	private float curTimer = 0;
	/**\
	 * 0: moving in right
	 * 1: stay for a while
	 * 2: movint out right
	 * 3: no chosen
	 * 4: chose boy
	 * 5: confirmation boy
	 * 6: chose girl
	 * 7: confirmation girl
	 **/
	private int status = 0;

	// Use this for initialization
	void Start () {
		box.transform.position = new Vector2 (0.5f * Screen.width, 0.5f * Screen.height);
		button_boy.transform.position = new Vector2 (0.5f * Screen.width, 0.75f * Screen.height);
		button_boy.GetComponent<RectTransform>().sizeDelta = new Vector2 (2 * Screen.width, 0.9f * Screen.height);
		button_girl.transform.position = new Vector2 (0.5f * Screen.width, 0.25f * Screen.height);
		button_girl.GetComponent<RectTransform>().sizeDelta = new Vector2 (2 * Screen.width, 0.9f * Screen.height);
		choose.transform.position = new Vector2 (-0.5f * Screen.width, 0.5f * Screen.height);
		showBgAllBlur ();
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevelAsync ("Title");
		}*/
		if (status == 0) {
			if (curTimer > stopTimer) {
				status = 1;
				choose.transform.position = new Vector2 (0.5f * Screen.width, 0.5f * Screen.height);
				curTimer = 0;
			} else {
				choose.transform.position += new Vector3 (Time.deltaTime / stopTimer * Screen.width, 0, 0);
				curTimer += Time.deltaTime;
			}
		} else if (status == 1) {
			if (curTimer > stopTimer2) {
				status = 2;
				curTimer = 0;
			} else {
				curTimer += Time.deltaTime;
			}
		} else if (status == 2) {
			if (curTimer > stopTimer) {
				status = 3;
				choose.transform.position = new Vector2 (1.5f * Screen.width, 0.5f * Screen.height);
				showBgNoBlur ();
			} else {
				choose.transform.position += new Vector3 (Time.deltaTime / stopTimer * Screen.width, 0, 0);
				curTimer += Time.deltaTime;
			}
		}
	}

	void showBgAllBlur() {
		bg_all_blur.SetActive (true);
		bg_no_blur.SetActive (false);
		bg_boy.SetActive (false);
		bg_girl.SetActive (false);
	}

	void showBgNoBlur() {
		bg_all_blur.SetActive (false);
		bg_no_blur.SetActive (true);
		bg_boy.SetActive (false);
		bg_girl.SetActive (false);
	}

	void showBgBoySelected() {
		bg_all_blur.SetActive (false);
		bg_no_blur.SetActive (false);
		bg_boy.SetActive (true);
		bg_girl.SetActive (false);
	}

	void showBgGirlSelected() {
		bg_all_blur.SetActive (false);
		bg_no_blur.SetActive (false);
		bg_boy.SetActive (false);
		bg_girl.SetActive (true);
	}

	public void onYesSelected() {
		if (status == 5) {
			PlayerPrefs.SetString ("playergender", "boy");
			Application.LoadLevelAsync ("WaitingRoom");
		}
		else if (status == 7) {
			PlayerPrefs.SetString ("playergender", "girl");
			Application.LoadLevelAsync ("WaitingRoom");
		}
	}

	public void onNoSelected() {
		status = 3;
		showBgNoBlur ();
		box.SetActive (false);
	}

	public void onBoySelected() {
		if (status == 4) {
			box.SetActive (true);
			status = 5;
		} else if (status != 5) {
			box.SetActive (false);
			showBgBoySelected ();
			status = 4;
		}
	}

	public void onGirlSelected() {
		if (status == 6) {
			box.SetActive (true);
			status = 7;
		} else if (status != 7) {
			box.SetActive (false);
			showBgGirlSelected ();
			status = 6;
		}
	}
}
