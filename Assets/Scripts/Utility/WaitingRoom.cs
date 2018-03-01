using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : MonoBehaviour {
	[SerializeField] Text text_info;
	[SerializeField] Text playerInfo;
	[SerializeField] Button startButton;
	// Use this for initialization
	void Start () {
		//Set Role
		if (CountBlue () > CountRed ()) {
			PhotonNetwork.player.SetTeam (PunTeams.Team.red);
		} else {
			PhotonNetwork.player.SetTeam (PunTeams.Team.blue);

		}
		StartCoroutine ("regularupdate");
		StartButtonActivation ();
	}
	
	public void StartRoombutton(){
		if (PhotonNetwork.playerList.Length > 1 || PhotonNetwork.offlineMode) {
			text_info.text = "Starting...";
			PhotonNetwork.LoadLevel ("Game");
		} else {
			text_info.text = "Not enough players";
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
			if (PhotonNetwork.room.PlayerCount > 1) {
				startButton.interactable = true;
			} else {
				startButton.interactable = false;
			}

		} else {
			startButton.gameObject.SetActive(false);
		}
	}
	void OnLeftRoom(){

		Application.LoadLevelAsync ("GameStart");

	}
	void OnPhotonPlayerConnected (PhotonPlayer newPlayer){
		UpdatePlayerInfo ();
		StartButtonActivation ();
	}
	void OnPhotonPlayerDisconnected (PhotonPlayer otherplayer){
		UpdatePlayerInfo ();
		StartButtonActivation ();
	}
}
