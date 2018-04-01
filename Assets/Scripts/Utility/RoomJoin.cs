using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class RoomJoin : MonoBehaviour {

	//Reference to info text
	[SerializeField] Text infoText;
	[SerializeField] Text statusText;
	[SerializeField] InputField input_roomname;
	//Properties for room
	private string roomName;
	private RoomInfo[] roomsList;
	private RoomOptions roomopt;


	bool inLobby = false;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetString ("roomname").Equals ("")) {
			JoinRoom ();
		} else {
			InitGame ();
		}
	}

	void InitGame() {
		inLobby = PhotonNetwork.insideLobby;
		PhotonNetwork.offlineMode = false;
		//Automatically sinc scene with master
		PhotonNetwork.automaticallySyncScene = true;

		//Set the room options
		roomopt = new RoomOptions(){IsVisible = true, MaxPlayers = 4};
		roomopt.PublishUserId = true;
		PhotonNetwork.player.NickName = PlayerPrefs.GetString ("playerName", "Player");

		//Connect
		statusText.color = Color.yellow;
		statusText.text = "Connecting...";
		//Connect to the server
		if (!PhotonNetwork.connected|| !inLobby) {
			PhotonNetwork.ConnectUsingSettings ("v4.2");
			statusText.color = Color.yellow;
			statusText.text = "Connecting...";
		} else {
			statusText.color = Color.green;

			statusText.text = "Connected";
		}
	}
	
	//Updating the room
	void UpdateRoom()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}


	public void JoinRoom(){
			
			
		roomopt = new RoomOptions(){IsVisible = false, MaxPlayers = 4};
		roomopt.PublishUserId = true;

		//Get The room list

		PhotonNetwork.offlineMode = false;
		//Joinning Lobby
		if (!PhotonNetwork.connected) {
			infoText.color = Color.red;
			infoText.text = "You're not connected yet";
			PhotonNetwork.ConnectUsingSettings ("v4.2");
			statusText.color = Color.yellow;
			statusText.text = "Connecting...";
		} else if (!inLobby) {
			PhotonNetwork.JoinLobby();
			infoText.color = Color.red;
			infoText.text = "You're not connected yet";
		} 
		else  {
			// Create Room
			if (PlayerPrefs.GetString ("roomname").Equals("") && input_roomname.text == "") {
				infoText.color = Color.red;
				infoText.text = "Name Invalid";
			} 
			else {

				if (PlayerPrefs.GetString ("roomname").Equals ("")) {
					roomName = input_roomname.text;
					PlayerPrefs.SetString ("roomname", roomName);
				} else {
					roomName = PlayerPrefs.GetString ("roomname");
				}
				PhotonNetwork.JoinOrCreateRoom (roomName, roomopt, TypedLobby.Default);
				infoText.color = Color.green;
				infoText.text = "Joining room...";
			}
		}

	}

	void OnJoinedRoom()
	{	
		//Pindah level
		PhotonNetwork.LoadLevel("WaitingRoom");
	}
	void OnPhotonJoinRoomFailed(){
		
		infoText.text = "Can't enter room";

	}

	void OnPhotonJoinOrCreateRoomFailed(){
		infoText.text = "Can't enter room";
	
	}

	void OnPhotonCreateRoomFailed(){
		infoText.text = "Can't create room";
	}
	void OnJoinedLobby(){
		inLobby = true;
		statusText.color = Color.green;
		statusText.text = "Connected";

	}
	void OnConnectedToMaster(){
		inLobby = true;
		statusText.color = Color.green;
		statusText.text = "Connected";
	}
	void OnDisconnectedFromPhoton(){

		PhotonNetwork.ConnectUsingSettings ("v4.2");
		statusText.color = Color.yellow;
		statusText.text = "Connecting...";
	}
	void OnConnectionFail(DisconnectCause cause) { 	
		statusText.color = Color.red;
		statusText.text = "Failed to connect";
	}

	void OnFailedToConnectToPhoton(DisconnectCause cause){
		statusText.color = Color.red;
		statusText.text = "Failed to connect";
	}
}
