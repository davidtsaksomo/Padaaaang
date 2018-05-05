using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHandler : MonoBehaviour {

	[SerializeField] GameObject box;
		// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			box.SetActive (true);
		}
	}

	public void yes(){
		PhotonNetwork.player.SetTeam (PunTeams.Team.none);
		if (PhotonNetwork.inRoom) {
			PhotonNetwork.LeaveRoom ();
		} else {
			Application.LoadLevel("Title");
		}
	}

	public void no(){
		box.SetActive (false);
	}
	void OnLeftRoom(){
		Application.LoadLevel("Title");

	}
}
