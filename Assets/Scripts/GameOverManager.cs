using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

	public void BackButton(){
		if (PhotonNetwork.inRoom) {
			PhotonNetwork.LeaveRoom ();
		} else {
			Application.LoadLevel("Title");
		}
	}

	void OnLeftRoom(){
		Application.LoadLevel("Title");

	}
}
