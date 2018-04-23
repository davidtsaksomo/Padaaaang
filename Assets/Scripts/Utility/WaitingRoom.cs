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
	[SerializeField] GameObject leftDoor;
	[SerializeField] GameObject rightDoor;
	public float stopTimer;
	public float stopTimer2;
	private float curTimer;
	/**
	 * Status:
	 * 0: don't close
	 * 1: closing
	 * 2: closed
	 **/
	private int status;

	// Use this for initialization
	void Start () {
		photonView = GetComponent<PhotonView> ();
		//Set Role
		if (CountWaiter () > CountChef ()) {
			PhotonNetwork.player.SetTeam (PunTeams.Team.red);
		} else {
			PhotonNetwork.player.SetTeam (PunTeams.Team.blue);
		}
		leftDoor.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width, Screen.height * 2.0f);
		rightDoor.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width, Screen.height * 2.0f);
		leftDoor.SetActive (false);
		rightDoor.SetActive (false);
		status = 0;
		StartCoroutine ("regularupdate");
		StartButtonActivation ();
	}

	void Update() {
		if (status == 1) {
			if (curTimer >= stopTimer) {
				status = 2;
				leftDoor.transform.position = new Vector3 (Screen.width / 4, leftDoor.transform.position.y, leftDoor.transform.position.z);
				rightDoor.transform.position = new Vector3 (Screen.width / 4 * 3, rightDoor.transform.position.y, rightDoor.transform.position.z);
				curTimer = 0;
			} else {
				leftDoor.transform.position += new Vector3 (Screen.width / 2 * Time.deltaTime / stopTimer, 0, 0);
				rightDoor.transform.position -= new Vector3 (Screen.width / 2 * Time.deltaTime / stopTimer, 0, 0);
				curTimer += Time.deltaTime;
			}
		} else if (status == 2) {
			if (curTimer >= stopTimer2) {
				status = 3;
				photonView.RPC ("GoToGame", PhotonTargets.All);
			} else {
				curTimer += Time.deltaTime;
			}
		}
	}

	private void animateDoorClose() {
		leftDoor.SetActive (true);
		rightDoor.SetActive (true);
		leftDoor.transform.position = new Vector3 (-Screen.width / 4, leftDoor.transform.position.y, leftDoor.transform.position.z);
		rightDoor.transform.position = new Vector3 (Screen.width / 4 * 5, rightDoor.transform.position.y, rightDoor.transform.position.z);
		curTimer = 0;
		status = 1;
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
			animateDoorClose ();
		} else {
			text_info.text = "Not enough players";
		}
	}

	[PunRPC]
	void GoToGame (){
		if (PhotonNetwork.player.GetTeam () == PunTeams.Team.blue) {
			//PhotonNetwork.LoadLevel ("GameWaiter");
			PhotonNetwork.LoadLevel ("GameWaiter (plus door)");
		} else {
			//PhotonNetwork.LoadLevel ("GameCook");
			PhotonNetwork.LoadLevel ("GameCook (plus door)");
		}
	}
	IEnumerator regularupdate(){
		UpdatePlayerInfo ();
		yield return new WaitForSeconds(5f);
		StartCoroutine ("regularupdate");
	}
	int CountChef(){
		int red = 0;
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.red) {
				red++;
			}
		}
		return red;
	}

	int CountWaiter(){
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
