using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelStart : MonoBehaviour {

	[SerializeField] Text success;
	[SerializeField] Text level;
	[SerializeField] Text startsin;
	[SerializeField] DoorMovementScript doorMovement;
	// Use this for initialization
	void Start () {
		level.text = "Level " + LevelController.level;
		if (LevelController.level > 1) {
			success.text = "Success!!";
			StartCoroutine ("CountDown", 5);

		} else {
			success.text = "";
			StartCoroutine ("CountDown", 3);


		}
	}
	
	IEnumerator CountDown(int time){
		while (time > 0) {
			startsin.text = "Starts in " + time+ "...";
			yield return new WaitForSeconds (1f);
			time--;
		}
		doorMovement.animateDoorClose ();

	}
}
