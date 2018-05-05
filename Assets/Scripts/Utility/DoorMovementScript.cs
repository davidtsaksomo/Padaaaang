using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovementScript : MonoBehaviour {
	[SerializeField] GameObject leftDoor;
	[SerializeField] GameObject rightDoor;
	public float stopTimer;
	public float stopTimer2;

	private float curTimer = 0;
	/**
	 * Status:
	 * 0: opening
	 * 1: opened
	 **/
	private int status;

	// Use this for initialization
	void Start () {
		leftDoor.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width, Screen.height * 2.0f);
		rightDoor.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width, Screen.height * 2.0f);
		leftDoor.SetActive (true);
		rightDoor.SetActive (true);
		leftDoor.transform.position = new Vector3 (Screen.width / 4, leftDoor.transform.position.y, leftDoor.transform.position.z);
		rightDoor.transform.position = new Vector3 (Screen.width / 4 * 3, rightDoor.transform.position.y, rightDoor.transform.position.z);
		status = 0;
	}

	void Update() {
		if (status == 0) {
			if (curTimer >= stopTimer) {
				status = 1;
				leftDoor.SetActive (false);
				rightDoor.SetActive (false);
			} else {
				leftDoor.transform.position -= new Vector3 (Screen.width / 2 * Time.deltaTime / stopTimer, 0, 0);
				rightDoor.transform.position += new Vector3 (Screen.width / 2 * Time.deltaTime / stopTimer, 0, 0);
				curTimer += Time.deltaTime;
			}
		} else if (status == 3) {
			if (curTimer >= stopTimer) {
				status = 4;
				leftDoor.transform.position = new Vector3 (Screen.width / 4, leftDoor.transform.position.y, leftDoor.transform.position.z);
				rightDoor.transform.position = new Vector3 (Screen.width / 4 * 3, rightDoor.transform.position.y, rightDoor.transform.position.z);
				curTimer = 0;
			} else {
				leftDoor.transform.position += new Vector3 (Screen.width / 2 * Time.deltaTime / stopTimer, 0, 0);
				rightDoor.transform.position -= new Vector3 (Screen.width / 2 * Time.deltaTime / stopTimer, 0, 0);
				curTimer += Time.deltaTime;
			}
		} else if (status == 4) {
			if (curTimer >= stopTimer2) {
				status = 5;
				if (LevelController.role == LevelController.ROLE_WAITER) {
					PhotonNetwork.LoadLevel ("GameWaiter");
				} else {
					PhotonNetwork.LoadLevel ("GameCook");
				}
			} else {
				curTimer += Time.deltaTime;
			}
		}
	}

	public void animateDoorClose() {
		leftDoor.SetActive (true);
		rightDoor.SetActive (true);
		leftDoor.transform.position = new Vector3 (-Screen.width / 4, leftDoor.transform.position.y, leftDoor.transform.position.z);
		rightDoor.transform.position = new Vector3 (Screen.width / 4 * 5, rightDoor.transform.position.y, rightDoor.transform.position.z);
		curTimer = 0;
		status = 3;
	}
}
