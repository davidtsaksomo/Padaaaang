using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

	void Start(){
		InGameMusic inGameMusic = GameObject.Find ("In-Game Music").GetComponent<InGameMusic> ();
		inGameMusic.stopMusic ();
	}

	public void BackButton(){
		PhotonNetwork.player.SetTeam (PunTeams.Team.none);
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
