using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : MonoBehaviour {
	[SerializeField] Text text_info;
	[SerializeField] Text playerInfo;
	[SerializeField] Button startButton;
	[SerializeField] Button backButton;
	[SerializeField] PhotonView photonView;
	// Use this for initialization
	void Start () {
		photonView = GetComponent<PhotonView> ();
		//Set Role
		if (CountBlue () > CountRed ()) {
			PhotonNetwork.player.SetTeam (PunTeams.Team.red);
		} else {
			PhotonNetwork.player.SetTeam (PunTeams.Team.blue);

		}
		StartCoroutine ("regularupdate");
		StartButtonActivation ();
	}

	public void BackButton(){
		if (PhotonNetwork.inRoom) {
			PhotonNetwork.LeaveRoom ();
		} else {
			Application.LoadLevel("Title");
		}
	}
	
	public void StartRoombutton(){
		if (PhotonNetwork.playerList.Length > 0 || PhotonNetwork.offlineMode) {
			text_info.text = "Starting...";
			photonView.RPC ("GoToGame", PhotonTargets.All);
		} else {
			text_info.text = "Not enough players";
		}
	}

	[PunRPC]
	void GoToGame (){
		if (PhotonNetwork.player.GetTeam () == PunTeams.Team.blue) {
			PhotonNetwork.LoadLevel ("GameWaiter");
		} else {
			PhotonNetwork.LoadLevel ("GameCook");
		}
	}
	IEnumerator regularupdate(){
		UpdatePlayerInfo ();
		yield return new WaitForSeconds(5f);
		StartCoroutine ("regularupdate");
	}
	int CountRed(){
		int red = 0;
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.red) {
				red++;
			}
		}
		return red;
	}

	int CountBlue(){
		int blue = 0;
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.blue) {
				blue++;
			}
		}
		return blue;
	}

	void UpdatePlayerInfo(){
		string playerlist;
		int number = 1;
		playerlist = "";
		if (!PhotonNetwork.offlineMode) {
			int count = 0;
			foreach (PhotonPlayer player in PhotonNetwork.playerList) {
				string team = "";
				if (player.GetTeam () == PunTeams.Team.blue) {
					team = "(Waiter)";
				} else if (player.GetTeam () == PunTeams.Team.red) {
					team = "(Chef)";

				} else {
					team = "(No Team)";
				}
				if (player.IsLocal) {
					playerlist = playerlist + (number + ". " + player.NickName + " "+ team+"\n");
				} else {
					playerlist = playerlist + (number + ". " + player.NickName +  " "+ team+"\n");
				}
				number++;
			
				count++;
			}
		} 
		playerInfo.text = playerlist;
	}
	void StartButtonActivation(){
		if (PhotonNetwork.isMasterClient) {
			if (PhotonNetwork.room.PlayerCount > 0) {
				startButton.interactable = true;
			} else {
				startButton.interactable = false;
			}

		} else {
			startButton.gameObject.SetActive(false);
		}
	}

	void OnDisconnectedFromPhoton(){
		Application.LoadLevel("Title");
	}
	void OnPhotonPlayerConnected (PhotonPlayer newPlayer){
		UpdatePlayerInfo ();
		StartButtonActivation ();
	}
	void OnLeftRoom(){
		Application.LoadLevel("Title");

	}
	void OnPhotonPlayerDisconnected (PhotonPlayer otherplayer){
		UpdatePlayerInfo ();
		StartButtonActivation ();
	}
}
